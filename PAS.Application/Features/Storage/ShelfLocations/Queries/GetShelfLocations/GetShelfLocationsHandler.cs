using Application.Features.Storage.ShelfLocations.Dtos;
using MediatR;

namespace Application.Features.Storage.ShelfLocations.Queries;

public class GetShelfLocationsQueryHandler : IRequestHandler<GetShelfLocationsQuery, Result<PaginatedList<ShelfLocationListDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetShelfLocationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<ShelfLocationListDto>>> Handle(GetShelfLocationsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ShelfLocations
            .Where(s => !s.IsDeleted)
            .AsNoTracking();

        // Apply filters
        if (request.WarehouseId.HasValue)
        {
            query = query.Where(s => s.WarehouseId == request.WarehouseId);
        }

        if (!string.IsNullOrWhiteSpace(request.Zone))
        {
            query = query.Where(s => s.Zone == request.Zone);
        }

        if (!string.IsNullOrWhiteSpace(request.BinType))
        {
            query = query.Where(s => s.BinType == request.BinType);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(s => s.IsActive == request.IsActive);
        }

        if (request.HasInventory.HasValue)
        {
            if (request.HasInventory.Value)
                query = query.Where(s => _context.InventoryStocks.Any(i => i.ShelfId == s.Id && i.CurrentQuantity > 0));
            else
                query = query.Where(s => !_context.InventoryStocks.Any(i => i.ShelfId == s.Id && i.CurrentQuantity > 0));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(s =>
                s.Aisle.Contains(request.SearchTerm) ||
                s.Rack.Contains(request.SearchTerm) ||
                s.ShelfNumber.Contains(request.SearchTerm) ||
                s.QRCodeValue.Contains(request.SearchTerm) ||
                (s.Zone != null && s.Zone.Contains(request.SearchTerm)));
        }

        // Project to DTO using subqueries - use string concatenation with + for EF Core translation
        var projectedQuery = query.Select(s => new ShelfLocationListDto
        {
            Id = s.Id,
            WarehouseName = _context.Warehouses.Where(w => w.Id == s.WarehouseId).Select(w => w.WarehouseName).FirstOrDefault() ?? string.Empty,
            FullAddress = s.Aisle + "-" + s.Rack + "-" + s.ShelfNumber,
            QRCodeValue = s.QRCodeValue,
            IsActive = s.IsActive,
            ItemCount = _context.InventoryStocks.Count(i => i.ShelfId == s.Id && i.CurrentQuantity > 0),
            TotalQuantity = _context.InventoryStocks.Where(i => i.ShelfId == s.Id).Sum(i => (int?)i.CurrentQuantity) ?? 0,
            Capacity = s.Capacity
        });

        var paginatedShelves = await projectedQuery
            .OrderBy(s => s.WarehouseName)
            .ThenBy(s => s.FullAddress)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return Result<PaginatedList<ShelfLocationListDto>>.Success(paginatedShelves);
    }
}
