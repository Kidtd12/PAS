using AutoMapper;
using MediatR;
using PAS.Application.Features.Requisition.StoreIssueVouchers.Queries.GetStoreIssueVoucherById;

namespace Application.Features.Requisition.StoreIssueVouchers.Queries;

public class GetStoreIssueVoucherByIdQueryHandler : IRequestHandler<GetStoreIssueVoucherByIdQuery, Result<StoreIssueVoucherDetailDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStoreIssueVoucherByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<StoreIssueVoucherDetailDto>> Handle(GetStoreIssueVoucherByIdQuery request, CancellationToken cancellationToken)
    {
        var siv = await _context.StoreIssueVouchers
            .Include(s => s.ServiceRequest)
                .ThenInclude(sr => sr.Requester)
                    .ThenInclude(r => r.Employee)
            .Include(s => s.IssuedBy)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, cancellationToken);

        if (siv == null)
        {
            throw new NotFoundException(nameof(Domain.Requisition.StoreIssueVoucher), request.Id);
        }

        var sivDto = _mapper.Map<StoreIssueVoucherDetailDto>(siv);

        // Get issued items from service request details
        var items = await _context.SR_Details
            .Include(d => d.Item)
            .Include(d => d.Shelf)
            .Where(d => d.SRId == siv.SRId && d.IssuedQty > 0)
            .Select(d => new SIVItemDetailDto
            {
                ItemId = d.ItemId,
                ItemName = d.Item != null ? d.Item.ItemName : string.Empty,
                SKU = d.Item != null ? d.Item.SKU : string.Empty,
                RequestedQty = d.RequestedQty,
                IssuedQty = d.IssuedQty,
                UnitOfMeasure = d.Item != null ? d.Item.UnitOfMeasure : string.Empty,
                ShelfId = d.ShelfId,
                ShelfLocation = d.Shelf != null ?
                    $"{d.Shelf.Aisle}-{d.Shelf.Rack}-{d.Shelf.ShelfNumber}" : string.Empty
            })
            .ToListAsync(cancellationToken);

        sivDto.Items = items;
        sivDto.TotalItems = items.Count;
        sivDto.TotalQuantity = items.Sum(i => i.IssuedQty);

        // Add service request summary
        if (siv.ServiceRequest != null)
        {
            sivDto.ServiceRequest = new ServiceRequestSummaryDto
            {
                Id = siv.ServiceRequest.Id,
                SRNumber = siv.ServiceRequest.SRNumber,
                RequestDate = siv.ServiceRequest.RequestDate,
                RequesterName = siv.ServiceRequest.Requester?.Employee?.FullName ?? "Unknown",
                Department = siv.ServiceRequest.Requester?.Employee?.Department ?? "Unknown"
            };
        }

        return Result<StoreIssueVoucherDetailDto>.Success(sivDto);
    }
}