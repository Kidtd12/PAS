using Application.Events;
using MediatR;

namespace Application.Features.Workflow.Approvers.Commands.AssignApprover;

public class AssignApproverCommandHandler : IRequestHandler<AssignApproverCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMediator _mediator;

    public AssignApproverCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMediator mediator)
    {
        _context = context;
        _currentUser = currentUser;
        _mediator = mediator;
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

        var user = await _context.UserLogins
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted && u.IsActive, cancellationToken);

        if (user == null)
        {
            return Result<Guid>.Failure("User not found or inactive.");
        }

        var existingApprover = workflow.Approvers?
            .FirstOrDefault(a => a.UserId == request.UserId && !a.IsDeleted);

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

        var approver = new Approver(request.WorkflowId, request.UserId, request.ApprovalLevel);

        _context.Approvers.Add(approver);
        await _context.SaveChangesAsync(cancellationToken);

        var auditTrail = new AuditTrail(
            _currentUser.UserGuid ?? Guid.Empty,
            "ASSIGN_APPROVER",
            nameof(Approver),
            approver.Id);
        _context.AuditTrails.Add(auditTrail);
        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new EntityCreatedEvent<Approver>(approver, _currentUser.UserGuid), cancellationToken);

        return Result<Guid>.Success(approver.Id);
    }
}