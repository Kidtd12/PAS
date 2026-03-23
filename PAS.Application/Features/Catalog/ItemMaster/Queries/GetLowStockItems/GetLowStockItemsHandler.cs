using MediatR;

namespace Application.Features.Catalog.ItemMasters.Queries.GetLowStockItems;

public class GetLowStockItemsHandler : IRequestHandler<GetLowStockItemsQuery, Result<List<LowStockItemDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetLowStockItemsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<LowStockItemDto>>> Handle(GetLowStockItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _context.ItemMasters
            .Include(i => i.InventoryStocks)
                .ThenInclude(s => s.Shelf)
            .Where(i => !i.IsDeleted)
            .ToListAsync(cancellationToken);

        var lowStockItems = items
            .Where(i => (i.InventoryStocks?.Sum(s => s.CurrentQuantity) ?? 0) <= i.MinStockLevel)
            .Select(i => new LowStockItemDto
            {
                ItemId = i.Id,
                ItemName = i.ItemName,
                SKU = i.SKU,
                CurrentStock = i.InventoryStocks?.Sum(s => s.CurrentQuantity) ?? 0,
                MinStockLevel = i.MinStockLevel,
                Deficit = i.MinStockLevel - (i.InventoryStocks?.Sum(s => s.CurrentQuantity) ?? 0),
                Locations = i.InventoryStocks?
                    .Where(s => s.CurrentQuantity > 0)
                    .Select(s => $"{s.Shelf.Aisle}-{s.Shelf.Rack}-{s.Shelf.ShelfNumber}")
                    .ToList() ?? new()
            })
            .OrderBy(i => i.Deficit)
            .ToList();

        return Result<List<LowStockItemDto>>.Success(lowStockItems);
    }
}