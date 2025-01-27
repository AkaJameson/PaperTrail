using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaperTrail.Storage.Entitys
{
    /// <summary>
    /// 随笔
    /// </summary>
    public class Essay : ICreationAudited
    {
        public int Id { get; set; }
        public string Content {  get; set; }
        public string CreatedBy {  get; set; }
        public DateTime CreatedTime { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }

    public class EssayConfiguration : IEntityTypeConfiguration<Essay>
    {
        public void Configure(EntityTypeBuilder<Essay> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Content).HasMaxLength(1000);
        }
    }

}
