using Microsoft.EntityFrameworkCore;
using PaperTrail.Storage.Entitys;
using Si.EntityFramework.Extension.Abstraction;
using Si.EntityFramework.Extension.DataBase;
using Si.EntityFramework.Extension.Entitys;
using Si.EntityFramework.Extension.Rbac.Entitys;

namespace PaperTrail.Storage
{
    public class BlogDbContext : ApplicationDbContext
    {
        public BlogDbContext(DbContextOptions options, ExtensionDbOptions optionsExtension, IUserInfo sessions = null) : base(options, optionsExtension, sessions)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Essay> Essays { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new EssayConfiguration());
            modelBuilder.ApplyConfiguration(new RoleUserConfiguration()); 
        }

    }
}
