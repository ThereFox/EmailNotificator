using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistense.Entitys;

namespace Persistense.Configurations;

public class NotificationEntityConfiguration : IEntityTypeConfiguration<NotificationEntity>
{
    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder
            .HasKey(ex => ex.Id);

        builder
            .Property(ex => ex.Id)
            .ValueGeneratedNever();

        builder
            .Property(ex => ex.Status)
            .HasColumnType("varchar")
            .IsRequired();

        builder
            .Property(ex => ex.CreatedAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder
            .Property(ex => ex.SendAt)
            .HasColumnType("timestamp")
            .IsRequired();

        builder
            .HasOne(ex => ex.Device)
            .WithMany(ex => ex.SendedNotification)
            .HasPrincipalKey(ex => ex.Id)
            .HasForeignKey(ex => ex.DeviceId)
            .IsRequired();

    }
}