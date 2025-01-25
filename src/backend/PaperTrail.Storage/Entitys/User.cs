using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaperTrail.Storage.Entitys
{
    /// <summary>
    /// 个人博客项目，只有一个类 一条数据
    /// </summary>
    public class User:Si.EntityFramework.PermGuard.Entitys.User
    {
        public string Name { get; set; }
        public string Account { get; set; }
        public string PasswordHash { get; set; }
    }
    public class UserRoleConfiguration : Si.EntityFramework.PermGuard.Entitys.RoleUserConfiguration
    {
        public override void Configure(EntityTypeBuilder<Si.EntityFramework.PermGuard.Entitys.User> builder)
        {
            base.Configure(builder);
        
        }
    }
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Account).HasMaxLength(50);
            builder.Property(x => x.PasswordHash).HasMaxLength(256);
        }
    }
}
