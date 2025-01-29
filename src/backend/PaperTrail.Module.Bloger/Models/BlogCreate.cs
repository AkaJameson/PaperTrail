namespace PaperTrail.Module.Bloger.Models
{
    public class BlogCreate
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<int> tagId { get; set; }
        public List<int> categoryId { get; set; }
        public bool isPublish { get; set; }

    }
}
