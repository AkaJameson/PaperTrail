using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Si.EntityFramework.Extension.Abstraction;

namespace PaperTrail.Storage.Entitys
{
    /// <summary>
    /// 个人博客项目，只有一个类 一条数据
    /// </summary>
    public class User : IUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string PasswordHash { get; set; }
        public string? AvatarPath { get; set; }
        public string? Github { get; set; }
        public string? Email { get; set; }
        public string? QQ { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Essay> Essays { get; set; }
    }
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Account).HasMaxLength(50).HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_bin");
            builder.Property(x => x.PasswordHash).HasMaxLength(256).HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_bin"); 
            builder.HasMany(x => x.Essays).WithOne(x => x.User).HasForeignKey(p => p.UserId);
            builder.HasMany(x => x.Blogs).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
