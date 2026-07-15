using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Infrastructure.Data.Configurations
{
    class HistoricalConfiguration : IEntityTypeConfiguration<Historical>
    {
      
        public void Configure(EntityTypeBuilder<Historical> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("HistoricalId");

            builder.Property(c => c.TableId).IsRequired();

            builder.Property(c => c.OriginId).IsRequired();

            builder.Property(c => c.Activity).IsRequired();

            builder.Property(c => c.OriginId).IsRequired();

            builder.Property(c => c.Date).IsRequired();

            builder.Property(c => c.Observation).IsRequired();

            builder.HasOne(typeof(HistoricalTable)).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(typeof(User)).WithMany()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
