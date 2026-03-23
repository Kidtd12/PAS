using Application.Features.TransferReturn.ReturnMaterialRequests.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Features.TransferReturn.ReturnMaterialRequests.Queries;

public class GetReturnRequestsQueryHandler : IRequestHandler<GetReturnRequestsQuery, Result<PaginatedList<ReturnListDto>>>
{
    public GetReturnRequestsQueryHandler(IApplicationDbContext context, IMapper mapper) { }

    public async Task<Result<PaginatedList<ReturnListDto>>> Handle(GetReturnRequestsQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var empty = new PaginatedList<ReturnListDto>(new List<ReturnListDto>(), 0, request.PageNumber, request.PageSize);
        return Result<PaginatedList<ReturnListDto>>.Success(empty);
    }
}