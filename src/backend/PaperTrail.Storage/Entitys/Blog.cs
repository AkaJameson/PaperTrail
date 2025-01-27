
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PaperTrail.Storage.Entitys
{
    public class Blog : ICreationAudited
    {
        public int Id { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Category> Categorys { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public bool IsPublish { get; set; }
        public DateTime CreatedTime { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder
              .HasMany(b => b.Tags)
              .WithMany(t => t.Blogs)
                .UsingEntity(
                    j => j.ToTable("BlogTags")
                );
            builder.HasMany(b => b.Comments).WithOne(c => c.Blog).HasForeignKey(c => c.BlogId);
            builder.HasMany(b => b.Categorys).WithMany(c => c.Blogs).UsingEntity(j => j.ToTable("BlogCategorys"));
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Title).HasMaxLength(100).IsRequired();
            builder.Property(b => b.Content).HasMaxLength(1000).IsRequired();
            builder.Property(b => b.CreatedBy).HasMaxLength(50).IsRequired();
            builder.Property(b => b.CreatedTime).IsRequired();
            builder.Property(b => b.IsPublish).IsRequired();
        }
    }
}
