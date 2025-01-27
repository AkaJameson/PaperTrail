using Microsoft.EntityFrameworkCore;
using PaperTrail.Storage.Entitys;
using Si.EntityFramework.Extension.DataBase;
using Si.EntityFramework.Extension.Entitys;
using Si.EntityFramework.PermGuard.Entitys;

namespace PaperTrail.Storage
{
    public class BlogDbContext : SiDbContext
    {
        public BlogDbContext(DbContextOptions options, SiDbContextOptions optionsExtension, ICurrentUser currentUser = null) : base(options, optionsExtension, currentUser)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Entitys.User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Essay> Essays { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new EssayConfiguration());
            modelBuilder.Entity<Si.EntityFramework.PermGuard.Entitys.User>()
            .HasDiscriminator<string>("UserType").HasValue<Si.EntityFramework.PermGuard.Entitys.User>("UserBase").HasValue<PaperTrail.Storage.Entitys.User>("BlogUser");
        }

    }
}
