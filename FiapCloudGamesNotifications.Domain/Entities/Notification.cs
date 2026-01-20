using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesNotifications.Domain.Entities;

public class Notification : EntityBase
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string Email { get; set; }
    public Guid UserId { get; set; }

    [SetsRequiredMembers]
    public Notification(Guid userId, string title, string message, string email) : base()
    {
        Title = title;
        Message = message;
        Email = email;
        UserId = userId;
    }
}