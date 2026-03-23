using Application.Common.Security;
using Application.Features.Storage.InventoryStock.Dtos;
using MediatR;
using Application.Features.Storage.InventoryStock.DTOs;

namespace Application.Features.Storage.InventoryStock.Queries;

[Authorize(Permissions = Permissions.Inventory.View)]
public record GetStockByShelfQuery(Guid ShelfId) : IRequest<Result<List<InventoryStockDto>>>;