namespace PaperTrail.Module.Bloger.Models
{
    public class UpdateCategoryRequest
    {
        public string CategoryName { get; set; }
        public string newCategoryName { get; set; }
        public string? newDescription { get; set; }
    }
}
