using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesNotifications.Application.Services.Interfaces;
using MassTransit;

namespace FiapCloudGamesNotifications.Api.Consumers;

public class SendNotificationOrderPlacedConsumer : IConsumer<OrderPlacedIntegrationEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<SendNotificationOrderPlacedConsumer> _logger;

    public SendNotificationOrderPlacedConsumer(INotificationService notificationService, ILogger<SendNotificationOrderPlacedConsumer> logger)
    {
        this._notificationService = notificationService;
        this._logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderPlacedIntegrationEvent> context)
    {
        _logger.LogInformation("Received OrderPlacedIntegrationEvent for OrderId {OrderId}.", context.Message?.OrderId);

        try
        {
            await _notificationService.SendNotificationAsync(
                context.Message.UserId,
                "Order Placed",
                $"Your order with ID {context.Message.OrderId} has been successfully placed."
            );

            _logger.LogInformation("Processed OrderPlacedIntegrationEvent for OrderId {OrderId}.", context.Message?.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing OrderPlacedIntegrationEvent for OrderId {OrderId}.", context.Message?.OrderId);
            throw;
        }
    }
}
