using FiapCloudGamesNotifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGamesNotifications.Infra.Data.Context;

public class ContextDb : DbContext
{
    public ContextDb(DbContextOptions<ContextDb> options)
    : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserNotificationProfile> UserNotificationProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextDb).Assembly);
    }
}
