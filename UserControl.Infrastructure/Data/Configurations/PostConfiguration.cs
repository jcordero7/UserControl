using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserControl.Core.Entities;


namespace UserControl.Infrastructure.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {

        public void Configure(EntityTypeBuilder<Post> builder)
        {
            //builder.HasKey(c => c.AlternateKey);
            //builder.Property(c => c.Name).HasMaxLength(200);

            //builder.HasKey(c => c.Id);
            //builder.Property(e => e.Id).HasColumnName("PostId");
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.Date).IsRequired();
            builder.Property(c => c.Description).IsRequired().HasMaxLength(1000);
            builder.Property(c => c.Image).HasMaxLength(500);

            // builder.HasMany<Commentary>().WithOne().HasForeignKey(c => c.PublicationId);

          ///  builder.HasOne<User>(c => c.User).WithMany(b => b.Posts).HasForeignKey(c => c.UserId);

        }

    }
}
