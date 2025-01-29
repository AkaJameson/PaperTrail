namespace PaperTrail.Module.Bloger.Models
{
    public class CommentRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
        public int? ParentId { get; set; }
    }
}
