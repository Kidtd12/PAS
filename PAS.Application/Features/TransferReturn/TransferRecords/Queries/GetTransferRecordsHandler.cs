using Application.Features.TransferReturn.TransferRecords.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Features.TransferReturn.TransferRecords.Queries;

public class GetTransferRecordsQueryHandler : IRequestHandler<GetTransferRecordsQuery, Result<PaginatedList<TransferListDto>>>
{
    public GetTransferRecordsQueryHandler(IApplicationDbContext context, IMapper mapper) { }

    public async Task<Result<PaginatedList<TransferListDto>>> Handle(GetTransferRecordsQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var empty = new PaginatedList<TransferListDto>(new List<TransferListDto>(), 0, request.PageNumber, request.PageSize);
        return Result<PaginatedList<TransferListDto>>.Success(empty);
    }
}