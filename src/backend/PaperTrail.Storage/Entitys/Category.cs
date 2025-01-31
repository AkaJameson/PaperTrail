using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaperTrail.Storage.Entitys
{
    public class Category : ICreationAudited
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).HasMaxLength(100).IsRequired().HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_bin"); 
            builder.Property(c => c.Description).HasMaxLength(200);
            builder.Property(c => c.CreatedBy).HasMaxLength(50).IsRequired();
            builder.Property(c => c.CreatedTime).IsRequired();
        }
    }
}
