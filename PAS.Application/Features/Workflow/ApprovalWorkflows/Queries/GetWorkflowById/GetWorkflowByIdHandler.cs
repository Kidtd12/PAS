using Application.Features.Workflow.ApprovalWorkflows.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Features.Workflow.ApprovalWorkflows.Queries.GetWorkflowById;

public class GetWorkflowByIdHandler : IRequestHandler<GetWorkflowByIdQuery, Result<ApprovalWorkflowDetailDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWorkflowByIdHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<ApprovalWorkflowDetailDto>> Handle(GetWorkflowByIdQuery request, CancellationToken cancellationToken)
    {
        var workflow = await _context.ApprovalWorkflows
            .Include(w => w.Approvers.Where(a => !a.IsDeleted))
                .ThenInclude(a => a.User)
                    .ThenInclude(u => u.Employee)
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == request.Id && !w.IsDeleted, cancellationToken);

        if (workflow == null)
        {
            throw new NotFoundException(nameof(ApprovalWorkflow), request.Id);
        }

        var workflowDto = _mapper.Map<ApprovalWorkflowDetailDto>(workflow);

        workflowDto.Approvers = workflow.Approvers?
            .OrderBy(a => a.ApprovalLevel)
            .Select(a => new WorkflowApproverDto
            {
                Id = a.Id,
                UserId = a.UserId,
                UserName = a.User?.Employee?.FullName ?? a.User?.Username ?? "Unknown",
                ApprovalLevel = a.ApprovalLevel,
                AssignedAt = a.CreatedAt
            })
            .ToList() ?? new();

        workflowDto.ApproversCount = workflowDto.Approvers.Count;

        return Result<ApprovalWorkflowDetailDto>.Success(workflowDto);
    }
}