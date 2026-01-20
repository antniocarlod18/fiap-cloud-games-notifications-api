using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGamesNotifications.Domain.Entities;

public class UserNotificationProfile : EntityBase
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public bool Active { get; set; }

    [SetsRequiredMembers]
    public UserNotificationProfile(Guid userId, string email) : base()
    {
        UserId = userId;
        Email = email;
        Active = true;
    }
}
