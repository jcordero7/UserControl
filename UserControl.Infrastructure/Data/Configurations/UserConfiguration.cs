using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using UserControl.Core.Entities;
using UserControl.Core.Enumerations;

namespace UserControl.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("UserId");

            //no se usa por ahora
            builder.Property(c => c.key).HasMaxLength(15);

            builder.Property(c => c.Email).IsRequired().HasMaxLength(30);
            builder.HasIndex(e => e.Email)
                .HasName("UI_UserEmail")
                .IsUnique();

            builder.Property(c => c.Password).IsRequired().HasMaxLength(200);

            builder.Property(c => c.Names).IsRequired().HasMaxLength(50);
            builder.Property(c => c.SurNames).IsRequired().HasMaxLength(50);

            builder.Property(c => c.BirthDate).IsRequired();
            
            builder.Property(c => c.Phone).HasMaxLength(10);

            builder.Property(c => c.State).HasMaxLength(30).IsRequired().HasConversion(
               x => x.ToString(),
               x => (UserState)Enum.Parse(typeof(UserState), x)
               );

            builder.Ignore(c => c.program);

            //  builder.Property .HasRequired(u => u.BillingAddress).WithRequiredDependent();

            //  builder.HasOne<Post>(c => c.Post).WithOne(s => s.User);

        }
    }
}
