using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Si.EntityFramework.PermGuard.Entitys;
using System.Reflection.Emit;

namespace PaperTrail.Storage.Entitys
{
    /// <summary>
    /// 个人博客项目，只有一个类 一条数据
    /// </summary>
    public class User : UserBase
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
        public virtual ICollection<Role> Roles { get; set; }
    }
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Account).HasMaxLength(50);
            builder.Property(x => x.PasswordHash).HasMaxLength(256);
            builder.HasMany(x => x.Essays).WithOne(x => x.User).HasForeignKey(p => p.UserId);
            builder.HasMany(x => x.Blogs).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasDiscriminator<string>("UserType").HasValue<User>("User");

        }
    }
}
