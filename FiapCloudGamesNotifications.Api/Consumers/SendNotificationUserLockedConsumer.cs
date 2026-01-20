using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesNotifications.Application.Services.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesNotifications.Api.Consumers
{
    public class SendNotificationUserLockedConsumer : IConsumer<UserLockedIntegrationEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<SendNotificationUserLockedConsumer> _logger;

        public SendNotificationUserLockedConsumer(INotificationService notificationService, ILogger<SendNotificationUserLockedConsumer> logger)
        {
            this._notificationService = notificationService;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<UserLockedIntegrationEvent> context)
        {
            _logger.LogInformation("Received UserLockedIntegrationEvent for user {UserId}.", context.Message?.UserId);

            try
            {
                await _notificationService.SendNotificationAsync(
                    context.Message.UserId,
                    "Account Locked", 
                    "Your account has been locked.",
                    context.Message.Email);

                _logger.LogInformation("Processed UserLockedIntegrationEvent for user {UserId}.", context.Message?.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing UserLockedIntegrationEvent for user {UserId}.", context.Message?.UserId);
                throw;
            }
        }
    }
}
