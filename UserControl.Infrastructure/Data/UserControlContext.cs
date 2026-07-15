using Microsoft.EntityFrameworkCore;
using UserControl.Core.Entities;
using UserControl.Infrastructure.Data.Configurations;

namespace UserControl.Infrastructure.Data
{
    public class UserControlContext : DbContext
    {
        public UserControlContext()
        {
        }

        public UserControlContext(DbContextOptions<UserControlContext> options)
            : base(options)
        {
        }

       // public DbSet<Post> Posts { get; set; }
       // public DbSet<Commentary> Commentarys { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Historical> Historical { get; set; }

        public DbSet<HistoricalTable> HistoricalTable { get; set; }

        public DbSet<Program> Program { get; set; }

        public DbSet<ProgramXUser> ProgramXUser { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<RoleXProgram> RoleXProgram { get; set; }

        public DbSet<UserAccess> UserAccess { get; set; }


        public DbSet<Security> Securities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            //modelBuilder.ApplyConfiguration(new PostConfiguration());
            //modelBuilder.ApplyConfiguration(new CommentaryConfiguration());

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new SecurityConfiguration());
            modelBuilder.ApplyConfiguration(new HistoricalConfiguration());
            modelBuilder.ApplyConfiguration(new HistoricalTableConfiguration());
            modelBuilder.ApplyConfiguration(new ProgramConfiguration());
            modelBuilder.ApplyConfiguration(new ProgramXUserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleXProgramConfiguration());
            modelBuilder.ApplyConfiguration(new UserAccessConfiguration());

            //en vez de registrarlos uno a uno
            //  modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Role>().HasData(
               new Role { Id = 1, Name = "Administrador", IsActive = true },
               new Role { Id = 2, Name = "ProfileAdmin", IsActive = true },
               new Role { Id = 3, Name = "Operator", IsActive = true },
               new Role { Id = 4, Name = "Consumer", IsActive = true }
            );

            modelBuilder.Entity<Program>().HasData(
             new Program { Id = 1, Key = "Ev", Name = "Eventos", Description = "Sistema para anuncio de eventos", IsActive = true }
            );

            modelBuilder.Entity<RoleXProgram>().HasData(
            new RoleXProgram { ProgramId = 1, RoleId = 1 },
            new RoleXProgram { ProgramId = 1, RoleId = 2 },
            new RoleXProgram { ProgramId = 1, RoleId = 3 }

           );

            base.OnModelCreating(modelBuilder);
        }

    }
}
