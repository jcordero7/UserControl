using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Infrastructure.Data.Configurations
{
    public class UserAccessConfiguration : IEntityTypeConfiguration<UserAccess>
    {
        public void Configure(EntityTypeBuilder<UserAccess> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("AccessId");

            builder.Property(c => c.UserId).IsRequired();

            builder.Property(c => c.Date).IsRequired();

            builder.Property(c => c.Token).IsRequired()
                .HasMaxLength(300);

            builder.Property(c => c.IsActive).IsRequired();

            builder.Property(c => c.OperatingSystem).IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Browser).IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.City).IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.State).IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.RefreshToken).IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.DateRefresh).IsRequired();

            builder.Property(c => c.KeepSession).IsRequired();

            builder.HasOne(typeof(User))
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            //ANALIZAR ESTAS
            builder.HasIndex(u => u.RefreshToken)
            .HasName("UI_RefreshToken")
            .IsUnique();

            builder.HasIndex(u => new { u.UserId, u.Token })
                .HasName("UI_Token")
                .IsUnique();

        }
    }
}
