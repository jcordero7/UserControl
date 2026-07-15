using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UserControl.Core.Entities;

namespace UserControl.Infrastructure.Data.Configurations 
{
    class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("RoleId");

            builder.Property(c => c.Name).IsRequired()
                .HasMaxLength(15);

            builder.Property(c => c.IsActive).IsRequired();

        }
    }
}
