using FiapCloudGamesNotifications.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiapCloudGamesNotifications.Application.Dtos
{
    public class NotificationResponse
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }

        public static implicit operator NotificationResponse?(Notification? notification)
        {
            if (notification == null) return null;

            return new NotificationResponse
            {
                Title = notification.Title,
                Message = notification.Message,
                UserId = notification.UserId,
                Email = notification.Email
            };
        }
    }
}
