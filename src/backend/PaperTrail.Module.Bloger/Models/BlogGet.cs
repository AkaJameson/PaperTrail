using Si.CoreHub.OperateResult.Abstraction;

namespace PaperTrail.Module.Bloger.Models
{
    public class BlogGet : IPage
    {
        public bool isPublish;

        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
    }
}
