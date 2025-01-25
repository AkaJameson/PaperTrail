using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaperTrail.Storage.Entitys
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public int Like { get; set; }
        public DateTime CreateTime { get; set; }
        public int CommnetId { get; set; }
        public int BlogId { get; set; }
        public int ParentId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual Comment Parent { get; set; }
        public virtual ICollection<Comment> Childrens { get; set; }
    }

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Content).HasMaxLength(1000);
            builder.Property(c => c.Name).HasMaxLength(50);
            builder.Property(c => c.Email).HasMaxLength(100);
            builder.Property(c => c.Like).HasDefaultValue(0);
            builder.Property(c => c.CreateTime).IsRequired();
            builder.HasMany(c=>c.Childrens).WithOne(c=>c.Parent).HasForeignKey(c=>c.ParentId);
        }
    }
   
}
