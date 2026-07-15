using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UserControl.Core.Entities;

namespace UserControl.Infrastructure.Data.Configurations
{
    class ProgramConfiguration : IEntityTypeConfiguration<Program>
    {
        public void Configure(EntityTypeBuilder<Program> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("ProgramId");

            builder.Property(c => c.Key)
               .IsRequired().HasMaxLength(2);

            builder.Property(c => c.Name)
                .IsRequired().HasMaxLength(50);

            builder.Property(c => c.Description)
               .IsRequired().HasMaxLength(200);

            builder.Property(c => c.IsActive)
               .IsRequired();

        }
    }
}
