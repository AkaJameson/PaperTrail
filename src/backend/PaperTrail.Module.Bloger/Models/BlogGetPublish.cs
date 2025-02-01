using Si.CoreHub.OperateResult.Abstraction;

namespace PaperTrail.Module.Bloger.Models
{
    public class BlogGetPublish : IPage
    {
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
    }
    public class BlogGet : IPage
    {
        public bool isPublish { get; set; } = true;
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
    }
}
