using PaperTrail.Module.Bloger.Models;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Module.Bloger.Services
{
    public interface IBlogService
    {
        /// <summary>
        /// 创建博客文章
        /// </summary>
        Task<Result> CreatePostAsync(BlogCreate post);

        /// <summary>
        /// 更新博客文章
        /// </summary>
        Task<Result> UpdatePostAsync(BlogUpdate post);
        /// <summary>
        /// 删除博客文章
        /// </summary>
        Task<Result> DeletePostAsync(int id);

        /// <summary>
        /// 获取博客文章详情
        /// </summary>
        Task<Result> GetPostAsync(int id);

        /// <summary>
        /// 获取博客文章列表
        /// </summary>
        Task<Result> GetPostListAsync(BlogGet blogGet);

        /// <summary>
        /// 根据标签获取博客文章列表
        /// </summary>
        Task<Result> GetPostListByTagAsync(string tag, BlogGet blogGet);
        
        /// <summary>
        /// 根据分类获取博客文章列表
        /// </summary>
        /// <param name="category"></param>
        /// <param name="blogGet"></param>
        /// <returns></returns>
        Task<Result> GetPostListByCategoryAsync(string category, BlogGet blogGet);

        /// <summary>
        /// 根据标签获取博客文章列表
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="blogGet"></param>
        /// <returns></returns>
        Task<Result> GetPostListByTagPublicAsync(string tag, BlogGetPublish blogGet);
        
        /// <summary>
        /// 根据分类获取博客文章列表
        /// </summary>
        /// <param name="category"></param>
        /// <param name="blogGet"></param>
        /// <returns></returns>
        Task<Result> GetPostListByCategoryPublicAsync(string category, BlogGetPublish blogGet);

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="blogGet"></param>
        /// <returns></returns>
        Task<Result> GetPostListPublicAsync(BlogGetPublish blogGet);
        /// <summary>
        /// 获取删除的文章列表
        /// </summary>
        /// <param name="blogGet"></param>
        /// <returns></returns>
        Task<Result> GetPostListDeleteAsync(BlogGet blogGet);
    }
}
