using FiapCloudGamesNotifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGamesNotifications.Infra.Data.Configurations;

public class NotificationsConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notification");
        builder.HasKey(o => o.Id);
        builder.Property(p => p.DateCreated).HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.DateUpdated).HasColumnType("DATETIME");

        builder.Property(o => o.Message).HasMaxLength(300);
        builder.Property(o => o.Title).HasMaxLength(300);
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(u => u.UserId)
            .IsRequired();
    }
}
