using FiapCloudGamesNotifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGamesNotifications.Infra.Data.Configurations;

public class UserNotificationProfileConfiguration : IEntityTypeConfiguration<UserNotificationProfile>
{
    public void Configure(EntityTypeBuilder<UserNotificationProfile> builder)
    {
        builder.ToTable("UserNotificationProfile");
        builder.HasKey(o => o.Id);
        builder.Property(p => p.DateCreated).HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.DateUpdated).HasColumnType("DATETIME");

        builder.Property(u => u.Active).IsRequired();
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(u => u.UserId)
            .IsRequired();
    }
}
