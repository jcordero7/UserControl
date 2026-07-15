using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UserControl.Core.Entities;

namespace UserControl.Infrastructure.Data.Configurations
{
    class RoleXProgramConfiguration : IEntityTypeConfiguration<RoleXProgram>
    {
        public void Configure(EntityTypeBuilder<RoleXProgram> builder)
        {
            builder.Ignore(c => c.Id);

            builder.HasKey(c => new { c.ProgramId, c.RoleId});

            //builder.Property(c => c.ProgramId).IsRequired();
            //builder.Property(c => c.RoleId).IsRequired();

            //builder.HasIndex(i => new { i.ProgramId, i.RoleId })
            //    .HasName("UI_RoleSystem");


            //****** ORIGINALES *****
            //builder.HasOne(typeof(Program))
            //   .WithMany()
            //   .OnDelete(DeleteBehavior.Restrict);


            //builder.HasOne(typeof(Role))
            //   .WithMany()
            //   .OnDelete(DeleteBehavior.Restrict);
            //****** ORIGINALES *****

            // Usando TypeOf (como lo tenías), esto está bien para la FK RoleId:
            builder.HasOne(typeof(Program))
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);


            // **Usando TypeOf (Tu implementación actual) y especificando la FK:**
            builder.HasOne(typeof(Role), "role") // El segundo argumento es el nombre de la propiedad de navegación
                .WithMany()
                // Especifica que esta relación debe usar RoleId como la clave foránea
                .HasForeignKey("RoleId") // <-- Esto indica a EF que use ProgramXUser.RoleId
                .OnDelete(DeleteBehavior.Restrict);

            // NOTA: Si usaste la primera opción (HasOne(c => c.Role)), EF Core es más inteligente
            // y suele entender mejor la propiedad de navegación.


        }
    }
}
