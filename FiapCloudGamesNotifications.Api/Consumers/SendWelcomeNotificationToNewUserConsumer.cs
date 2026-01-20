using FiapCloudGames.Contracts.IntegrationEvents;
using FiapCloudGamesNotifications.Application.Services.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesNotifications.Api.Consumers;

public class SendWelcomeNotificationToNewUserConsumer : IConsumer<UserCreatedIntegrationEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<SendWelcomeNotificationToNewUserConsumer> _logger;

    public SendWelcomeNotificationToNewUserConsumer(INotificationService notificationService, ILogger<SendWelcomeNotificationToNewUserConsumer> logger)
    {
        this._notificationService = notificationService;
        this._logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
    {
        _logger.LogInformation("Received UserCreatedIntegrationEvent for UserId {UserId}.", context.Message.UserId);

        try
        {
            await _notificationService.SendNotificationAsync(
                context.Message.UserId,
                "Welcome to Fiap Cloud Games!",
                $"Hello {context.Message.Name}, welcome to Fiap Cloud Games! " +
                $"We're excited to have you on board. " +
                $"Unlock your account using your email and temporary password: {context.Message.Password}",
                context.Message.Email
            );

            _logger.LogInformation("Processed UserCreatedIntegrationEvent for UserId {UserId}.", context.Message.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing UserCreatedIntegrationEvent for UserId {UserId}.", context.Message.UserId);
            throw;
        }
    }
}
