using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserControl.Core.Entities;

namespace UserControl.Infrastructure.Data.Configurations
{
    public class CommentaryConfiguration : IEntityTypeConfiguration<Commentary>
    {
        public void Configure(EntityTypeBuilder<Commentary> builder)
        {
            //para mapear la tabla si queremos que la tabla tenga un nombre difente a nuestro modelo
            //builder.ToTable("Comentario");

            ////builder.HasKey(c => c.Id);
            ////builder.Property(c => c.Id).HasColumnName("ComentaryId");
          //  builder.Property(c => c.PublicationId).IsRequired();
            builder.Property(c => c.UserId).IsRequired();

            //cuando queremos mapear el nombre de la propiedad con un campo en la tabla que tiene un nombre diferente
           // builder.Property(c => c.UserId).HasColumnName("idusuario").IsRequired();

            builder.Property(c => c.Description).IsRequired().HasMaxLength(500);
            builder.Property(c => c.Date).IsRequired();
            builder.Property(c => c.Active).IsRequired();

           // builder.HasOne<Post>().WithMany().HasForeignKey(c => c.PublicationId);

            builder.HasOne<Post>(c => c.Post).WithMany(b => b.Comments).HasForeignKey(c => c.PublicationId);

          //  builder.HasOne<User>(c => c.User).WithMany(b => b.Commentaries).HasForeignKey(c => c.UserId);

        }
    }
}
