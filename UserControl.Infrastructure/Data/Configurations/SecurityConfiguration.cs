using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserControl.Core.Entities;
using UserControl.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Infrastructure.Data.Configurations
{
   public class SecurityConfiguration : IEntityTypeConfiguration<Security>
    {

        public void Configure(EntityTypeBuilder<Security> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasColumnName("SecurityId");
            builder.Property(c => c.User).IsRequired().HasMaxLength(50);
            builder.Property(c => c.UserName).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Password).IsRequired().HasMaxLength(200);

            builder.Property(c => c.Role).HasMaxLength(15).IsRequired().HasConversion(
                x => x.ToString(),
                x => (RoleType)Enum.Parse(typeof(RoleType), x)
                );

          //  builder.HasOne<User>(c => c.User).WithMany(b => b.Posts).HasForeignKey(c => c.UserId);

        }

       
    }
}
