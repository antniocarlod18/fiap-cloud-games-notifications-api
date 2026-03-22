using FiapCloudGamesNotifications.Application.Dtos;
using FiapCloudGamesNotifications.Application.Services.Interfaces;
using FiapCloudGamesNotifications.Domain.Entities;
using FiapCloudGamesNotifications.Domain.Repositories;
using MassTransit;
using MassTransit.Transports;
using Microsoft.Extensions.Logging;

namespace FiapCloudGamesNotifications.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public NotificationService(IUnitOfWork unitOfWork, ILogger<NotificationService> logger, ISendEndpointProvider sendEndpointProvider)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task SendNotificationAsync(Guid userId, string title, string message, string? email = null)
        {
            _logger.LogInformation("Preparing to send notification to user {UserId} with title \"{Title}\".", userId, title);

            if (string.IsNullOrWhiteSpace(email))
            {
                var userProfile = await _unitOfWork.UserNotificationProfileRepo.GetByUserAsync(userId);
                if (userProfile == null || !userProfile.Active)
                {
                    _logger.LogWarning("No active notification profile found for user {UserId}. Notification will not be sent.", userId);
                    return;
                }
                email = userProfile.Email;
            }

            var notification = new Notification(userId, title, message, email);

            _logger.LogInformation("Sending notification to {Email} identified by {UserId} with title \"{Title}\": {Message}",
                email, userId, title, message);

            await SendNotificationToEmailAsync(notification);

            await _unitOfWork.NotificationsRepo.AddAsync(notification);

            await _unitOfWork.Commit();

            _logger.LogInformation("Notification for user {UserId} persisted successfully.", userId);
        }

        public async Task<IList<NotificationResponse?>> GetByUserIdAsync(Guid userId)
        {
            var listOfNotifications = await _unitOfWork.NotificationsRepo.GetByUserIdAsync(userId);

            if (listOfNotifications == null || !listOfNotifications.Any())
            {
                _logger.LogInformation("No notifications found");
                return [];
            }

            _logger.LogInformation("Retrieved {Count} notifications", listOfNotifications.Count);
            return listOfNotifications.Select(x => (NotificationResponse?)x).ToList();
        }

        public async Task SendNotificationToEmailAsync(Notification notification)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(
                new Uri("queue:notification-queue")
            );

            await endpoint.Send(new
            {
                notification.Email,
                notification.Title,
                notification.Message
            });
        }
    }
}
