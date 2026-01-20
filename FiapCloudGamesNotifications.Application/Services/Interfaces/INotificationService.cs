using FiapCloudGamesNotifications.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesNotifications.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Guid userId, string title, string message, string? email=null);
        Task<IList<NotificationResponse?>> GetByUserIdAsync(Guid userId);
    }
}
