using Application.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;

namespace Application.Features.Workflow.Approvers.Commands.AssignApprover;

public class AssignApproverCommandHandler : IRequestHandler<AssignApproverCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public AssignApproverCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMediator mediator,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _currentUser = currentUser;
        _mediator = mediator;
        _userManager = userManager;
    }

    public async Task<Result<Guid>> Handle(AssignApproverCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _context.ApprovalWorkflows
            .Include(w => w.Approvers)
            .FirstOrDefaultAsync(w => w.Id == request.WorkflowId && !w.IsDeleted, cancellationToken);

        if (workflow == null)
        {
            return Result<Guid>.Failure("Workflow not found.");
        }

        // 1) Try legacy UserLogin by provided id (backward compatible)
        var user = await _context.UserLogins
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted && u.IsActive, cancellationToken);

        // 2) If not found, resolve by ASP.NET Identity user id and bridge to UserLogin
        if (user == null)
        {
            var identityUserId = request.UserId.ToString();
            var identityUser = await _userManager.FindByIdAsync(identityUserId);

            if (identityUser == null || !identityUser.IsActive)
            {
                return Result<Guid>.Failure("User not found or inactive.");
            }

            user = await _context.UserLogins
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.AspNetUserId == identityUserId && !u.IsDeleted && u.IsActive, cancellationToken);

            if (user == null)
            {
                // Find or create employee record to satisfy legacy UserLogin FK
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => !e.IsDeleted &&
                        ((e.Email != null && identityUser.Email != null && e.Email == identityUser.Email) ||
                         e.EmployeeCode == (identityUser.UserName ?? string.Empty)), cancellationToken);

                if (employee == null)
                {
                    var code = string.IsNullOrWhiteSpace(identityUser.UserName)
                        ? $"EMP-{Guid.NewGuid().ToString("N")[..8]}"
                        : identityUser.UserName;

                    var exists = await _context.Employees.AnyAsync(e => e.EmployeeCode == code && !e.IsDeleted, cancellationToken);
                    if (exists)
                    {
                        code = $"EMP-{Guid.NewGuid().ToString("N")[..8]}";
                    }

                    employee = new Domain.Users.Employee(
                        code,
                        identityUser.FullName ?? identityUser.UserName ?? "User",
                        "General");

                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // Find or create a legacy role for compatibility
                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => !r.IsDeleted && r.RoleName == "Approver", cancellationToken)
                    ?? await _context.Roles.FirstOrDefaultAsync(r => !r.IsDeleted, cancellationToken);

                if (role == null)
                {
                    role = new Domain.Users.Role("Approver", "Auto-created compatibility role");
                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                user = new Domain.Users.UserLogin(
                    employee.Id,
                    identityUser.UserName ?? identityUser.Email ?? identityUser.Id,
                    identityUser.PasswordHash ?? string.Empty,
                    role.Id,
                    identityUser.Id);

                _context.UserLogins.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        var approverUserId = user.Id;

        var existingApprover = workflow.Approvers?
            .FirstOrDefault(a => a.UserId == approverUserId && !a.IsDeleted);

        if (existingApprover != null)
        {
            return Result<Guid>.Failure("User is already assigned to this workflow.");
        }

        var existingLevel = workflow.Approvers?
            .Any(a => a.ApprovalLevel == request.ApprovalLevel && !a.IsDeleted) == true;

        if (existingLevel)
        {
            return Result<Guid>.Failure($"Approval level {request.ApprovalLevel} is already assigned.");
        }

        var approver = new Approver(request.WorkflowId, approverUserId, request.ApprovalLevel);

        _context.Approvers.Add(approver);
        await _context.SaveChangesAsync(cancellationToken);

        if (_currentUser.UserGuid.HasValue)
        {
            var auditTrail = new AuditTrail(
                _currentUser.UserGuid.Value,
                "ASSIGN_APPROVER",
                nameof(Approver),
                approver.Id);
            _context.AuditTrails.Add(auditTrail);
            await _context.SaveChangesAsync(cancellationToken);
        }

        await _mediator.Publish(new EntityCreatedEvent<Approver>(approver, _currentUser.UserGuid), cancellationToken);

        return Result<Guid>.Success(approver.Id);
    }
}