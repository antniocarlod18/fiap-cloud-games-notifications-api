using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesNotifications.Application.Services.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesNotifications.Api.Consumers
{
    public class SendNotificationPaymentOrderProcessedConsumer : IConsumer<PaymentProcessedIntegrationEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<SendNotificationPaymentOrderProcessedConsumer> _logger;

        public SendNotificationPaymentOrderProcessedConsumer(INotificationService notificationService, ILogger<SendNotificationPaymentOrderProcessedConsumer> logger)
        {
            this._notificationService = notificationService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentProcessedIntegrationEvent> context)
        {
            _logger.LogInformation("Received PaymentProcessedIntegrationEvent for OrderId {OrderId}.", context.Message?.OrderId);

            try
            {
                await _notificationService.SendNotificationAsync(
                    context.Message.UserId,
                    "Payment Processed",
                    $"Your payment for order ID {context.Message.OrderId} has been successfully processed." +
                    $"The order will be completed and the games will be added to your Library!"
                );

                _logger.LogInformation("Processed PaymentProcessedIntegrationEvent for OrderId {OrderId}.", context.Message?.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PaymentProcessedIntegrationEvent for OrderId {OrderId}.", context.Message?.OrderId);
                throw;
            }
        }
    }
}
