using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UserControl.Core.Entities;

namespace UserControl.Infrastructure.Data.Configurations
{
    class HistoricalTableConfiguration : IEntityTypeConfiguration<HistoricalTable>
    {
        public void Configure(EntityTypeBuilder<HistoricalTable> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("TableId");

            builder.Property(c => c.Name).IsRequired()
                .HasMaxLength(40);

            builder.Property(c => c.Description)
                .HasMaxLength(200);

        }
    }
}
