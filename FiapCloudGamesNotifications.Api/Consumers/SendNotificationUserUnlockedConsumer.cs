using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesNotifications.Application.Services.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesNotifications.Api.Consumers
{
    public class SendNotificationUserUnlockedConsumer : IConsumer<UserUnlockedIntegrationEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<SendNotificationUserUnlockedConsumer> _logger;

        public SendNotificationUserUnlockedConsumer(INotificationService notificationService, ILogger<SendNotificationUserUnlockedConsumer> logger)
        {
            this._notificationService = notificationService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<UserUnlockedIntegrationEvent> context)
        {
            _logger.LogInformation("Received UserUnlockedIntegrationEvent for user {UserId}.", context.Message?.UserId);

            try
            {
                await _notificationService.SendNotificationAsync(
                    context.Message.UserId,
                    "Account Unlocked",
                    "Your account has been successfully unlocked. You can now log in and continue enjoying our services.",
                    context.Message.Email
                );

                _logger.LogInformation("Processed UserUnlockedIntegrationEvent for user {UserId}.", context.Message?.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing UserUnlockedIntegrationEvent for user {UserId}.", context.Message?.UserId);
                throw;
            }
        }
    }
}
