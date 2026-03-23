using MediatR;

namespace Application.Features.Common.Notifications.Commands.CreateNotification;

public record CreateNotificationCommand : IRequest<Result<Guid>>
{
    public Guid UserId { get; init; }
    public string Message { get; init; } = string.Empty;
}

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMediator _mediator;

    public CreateNotificationCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IMediator mediator)
    {
        _context = context;
        _currentUser = currentUser;
        _mediator = mediator;
    }

    public async Task<Result<Guid>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.UserLogins
            .FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted && u.IsActive, cancellationToken);

        if (user == null)
        {
            return Result<Guid>.Failure("User not found or inactive.");
        }

        var notification = new Domain.Common.Notification(request.UserId, request.Message);

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(notification.Id);
    }
}