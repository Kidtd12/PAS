using Application.Features.Requisition.ServiceRequests.Dtos;
using MediatR;

namespace Application.Features.Requisition.ServiceRequests.Queries;

public class GetServiceRequestsQueryHandler : IRequestHandler<GetServiceRequestsQuery, Result<PaginatedList<ServiceRequestListDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetServiceRequestsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<Result<PaginatedList<ServiceRequestListDto>>> Handle(GetServiceRequestsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ServiceRequests
            .Include(s => s.Requester)
                .ThenInclude(r => r.Employee)
            .Include(s => s.Details)
            .Include(s => s.StoreIssueVoucher)
            .Where(s => !s.IsDeleted)
            .AsNoTracking();

        // Apply filters
        if (request.MyRequests == true && _currentUser.UserGuid.HasValue)
        {
            query = query.Where(s => s.RequesterId == _currentUser.UserGuid);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(s => s.Status == request.Status);
        }

        if (request.RequesterId.HasValue)
        {
            query = query.Where(s => s.RequesterId == request.RequesterId);
        }

        if (!string.IsNullOrWhiteSpace(request.Department))
        {
            query = query.Where(s => s.Requester.Employee.Department == request.Department);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(s => s.RequestDate >= request.FromDate);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(s => s.RequestDate <= request.ToDate);
        }

        // Project to DTO
        var projectedQuery = query.Select(s => new ServiceRequestListDto
        {
            Id = s.Id,
            SRNumber = s.SRNumber,
            RequestDate = s.RequestDate,
            RequesterName = s.Requester != null && s.Requester.Employee != null ?
                s.Requester.Employee.FullName : "Unknown",
            Department = s.Requester != null && s.Requester.Employee != null ?
                s.Requester.Employee.Department : "Unknown",
            Status = s.Status,
            ItemCount = s.Details != null ? s.Details.Count(d => !d.IsDeleted) : 0,
            TotalQuantity = s.Details != null ? s.Details.Where(d => !d.IsDeleted).Sum(d => d.RequestedQty) : 0,
            IssuedQuantity = s.Details != null ? s.Details.Where(d => !d.IsDeleted).Sum(d => d.IssuedQty) : 0,
            HasSIV = s.StoreIssueVoucher != null && !s.StoreIssueVoucher.IsDeleted
        });

        var paginatedRequests = await projectedQuery
            .OrderByDescending(s => s.RequestDate)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return Result<PaginatedList<ServiceRequestListDto>>.Success(paginatedRequests);
    }
}}