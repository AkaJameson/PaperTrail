namespace PaperTrail.Module.Bloger.Models
{
    public class BlogUpdate
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 标签列表
        /// </summary>
        public List<string> Tags { get; set; }
        /// <summary>
        /// 分类列表
        /// </summary>
        public List<string> Categorys { get; set; }
    }

}
