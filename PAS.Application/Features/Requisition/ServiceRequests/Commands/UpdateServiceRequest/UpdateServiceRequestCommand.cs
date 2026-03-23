using Application.Common.Security;
using Application.Events;
using MediatR;

namespace Application.Features.Requisition.ServiceRequests.Commands;

[Authorize(Permissions = Permissions.Requisitions.Edit)]
public record UpdateServiceRequestCommand : IRequest<Result>
{
    public Guid Id { get; init; }
    public List<ServiceRequestItemDto> Items { get; init; } = new();
    public string? Remarks { get; init; }
}