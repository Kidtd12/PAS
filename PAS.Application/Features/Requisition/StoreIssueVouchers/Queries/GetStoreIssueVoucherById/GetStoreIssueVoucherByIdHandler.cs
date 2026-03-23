using Application.Features.Requisition.StoreIssueVouchers.Dtos;
using AutoMapper;
using MediatR;

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
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, cancellationToken);

        if (siv == null)
        {
            throw new NotFoundException(nameof(Domain.Requisition.StoreIssueVoucher), request.Id);
        }

        var sivDto = _mapper.Map<StoreIssueVoucherDetailDto>(siv);
        sivDto.Items = new List<SIVItemDetailDto>();
        sivDto.TotalItems = 0;
        sivDto.TotalQuantity = 0;
        sivDto.ServiceRequest = new ServiceRequestSummaryDto
        {
            Id = siv.SRId,
            SRNumber = string.Empty,
            RequestDate = siv.IssueDate,
            RequesterName = string.Empty,
            Department = string.Empty
        };

        return Result<StoreIssueVoucherDetailDto>.Success(sivDto);
    }
}