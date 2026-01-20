using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesNotifications.Application.Services.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesNotifications.Api.Consumers;

public class CreateUserProfileConsumer : IConsumer<UserCreatedIntegrationEvent>
{
    private readonly IUserNotificationProfileService _userNotificationProfileService;
    private readonly ILogger<CreateUserProfileConsumer> _logger;

    public CreateUserProfileConsumer(IUserNotificationProfileService userNotificationProfileService, ILogger<CreateUserProfileConsumer> logger)
    {
        this._userNotificationProfileService = userNotificationProfileService;
        this._logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
    {
        _logger.LogInformation("Received UserCreatedIntegrationEvent for user {UserId}.", context.Message?.UserId);

        try
        {
            await _userNotificationProfileService.CreateUserNotificationProfileAsync(
                context.Message.UserId, context.Message.Email);

            _logger.LogInformation("Created user notification profile for user {UserId}.", context.Message?.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user profile for user {UserId}.", context.Message?.UserId);
            throw;
        }
    }
}
