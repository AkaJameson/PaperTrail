namespace PaperTrail.Module.Blog.Services
{
    public interface IBlogService
    {
        /// <summary>
        /// 创建博客文章
        /// </summary>
        Task<int> CreatePostAsync(BlogPost post);

        /// <summary>
        /// 更新博客文章
        /// </summary>
        Task UpdatePostAsync(BlogPost post);

        /// <summary>
        /// 删除博客文章
        /// </summary>
        Task DeletePostAsync(int id);

        /// <summary>
        /// 获取博客文章详情
        /// </summary>
        Task<BlogPost> GetPostAsync(int id);

        /// <summary>
        /// 获取博客文章列表
        /// </summary>
        Task<List<BlogPost>> GetPostListAsync(int pageIndex, int pageSize);

        /// <summary>
        /// 根据标签获取博客文章列表
        /// </summary>
        Task<List<BlogPost>> GetPostListByTagAsync(string tag, int pageIndex, int pageSize);

        /// <summary>
        /// 获取所有标签
        /// </summary>
        Task<List<string>> GetAllTagsAsync();

        /// <summary>
        /// 添加评论
        /// </summary>
        Task AddCommentAsync(BlogComment comment);

        /// <summary>
        /// 获取文章评论列表
        /// </summary>
        Task<List<BlogComment>> GetCommentsAsync(int postId);
    }
}

public class BlogPost
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
    /// 作者
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime PublishTime { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// 标签列表
    /// </summary>
    public List<string> Tags { get; set; }

    /// <summary>
    /// 阅读量
    /// </summary>
    public int ViewCount { get; set; }
}

public class BlogComment
{
    /// <summary>
    /// 评论ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 文章ID
    /// </summary>
    public int PostId { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 评论者
    /// </summary>
    public string Commenter { get; set; }

    /// <summary>
    /// 评论时间
    /// </summary>
    public DateTime CommentTime { get; set; }
}
