using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Infrastructure.Data.Configurations
{
    class ProgramXUserConfiguration : IEntityTypeConfiguration<ProgramXUser>
    {
        public void Configure(EntityTypeBuilder<ProgramXUser> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId).IsRequired();

            builder.Property(c => c.ProgramId).IsRequired();

            builder.Property(c => c.RoleId).IsRequired();

            //builder.HasOne(typeof(User))
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);


            //SE DEBE ELIMINAR ESTA RELACION PORQUE EF YA LA CREA CON LAS PROPIEDADES VIRTUALES
            //*****************************************
            //builder.HasOne(r => r.user)
            //.WithMany().HasForeignKey(x => x.UserId)
            //.OnDelete(DeleteBehavior.Restrict);
            //*****************************************

            builder.HasOne(r => r.roleXProgram)
             .WithMany().HasForeignKey(x => new { x.ProgramId, x.RoleId})
             .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
