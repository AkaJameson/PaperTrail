using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperTrail.Module.Bloger.Models;
using PaperTrail.Module.Bloger.Services;
using PaperTrail.Storage.Enums;
using Si.CoreHub;
using Si.CoreHub.OperateResult;
using Si.EntityFramework.Extension.Rbac.Handlers;

namespace PaperTrail.Module.Bloger.Controllers
{
    /// <summary>
    /// 博客控制器，用于处理与博客帖子相关的API请求。
    /// </summary>
    [ApiController]
    public class BlogController : DefaultController
    {
        /// <summary>
        /// 博客服务接口，用于执行与博客帖子相关的操作。
        /// </summary>
        private readonly IBlogService blogService;
        /// <summary>
        /// 博客控制器的构造函数。
        /// </summary>
        /// <param name="blogService">博客服务接口的实例。</param>
        public BlogController(IBlogService blogService)
        {
            this.blogService = blogService;
        }
        /// <summary>
        /// 获取博客帖子列表。
        /// </summary>
        /// <param name="blogGet">包含查询参数的模型。</param>
        /// <returns>包含帖子列表的查询结果。</returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> GetPostList([FromBody] BlogGet blogGet)
        {
            return await blogService.GetPostListAsync(blogGet);
        }
        /// <summary>
        /// 根据ID获取博客帖子详情。
        /// </summary>
        /// <param name="id">帖子的唯一标识符。</param>
        /// <returns>指定ID的帖子详情。</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<Result> GetPostById([FromQuery] int id)
        {
            return await blogService.GetPostAsync(id);
        }
        /// <summary>
        /// 根据类别获取博客帖子列表。
        /// </summary>
        /// <param name="category">帖子的类别。</param>
        /// <param name="blogGet">包含查询参数的模型。</param>
        /// <returns>指定类别的帖子列表。</returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> GetPostListByCategory([FromQuery] string category, [FromBody] BlogGet blogGet)
        {
            return await blogService.GetPostListByCategoryAsync(category, blogGet);
        }
        /// <summary>
        /// 根据标签获取博客帖子列表。
        /// </summary>
        /// <param name="tag">帖子的标签。</param>
        /// <param name="blogGet">包含查询参数的模型。</param>
        /// <returns>指定标签的帖子列表。</returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> GetPostListByTag([FromQuery] string tag, [FromBody] BlogGet blogGet)
        {
            return await blogService.GetPostListByTagAsync(tag, blogGet);
        }
        /// <summary>
        /// 删除指定ID的博客帖子。
        /// </summary>
        /// <param name="id">帖子的唯一标识符。</param>
        /// <returns>删除操作的结果。</returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> DeletePostAsync(int id)
        {
            return await blogService.DeletePostAsync(id);
        }
        /// <summary>
        /// 更新博客帖子的信息。
        /// </summary>
        /// <param name="blogUpdate">包含更新信息的模型。</param>
        /// <returns>更新操作的结果。</returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> UpdatePostAsync([FromBody] BlogUpdate blogUpdate)
        {
            return await blogService.UpdatePostAsync(blogUpdate);
        }
        /// <summary>
        /// 创建新的博客帖子。
        /// </summary>
        /// <param name="blogCreate">包含新帖子信息的模型。</param>
        /// <returns>创建操作的结果。</returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> CreatePostAsync([FromBody] BlogCreate blogCreate)
        {
            return await blogService.CreatePostAsync(blogCreate);
        }
        /// <summary>
        /// 获取公开博客帖子列表。
        /// </summary>
        /// <param name="blogGet"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<Result> GetPostListPublic([FromBody] BlogGetPublish blogGet)
        {
            return await blogService.GetPostListPublicAsync(blogGet);
        }
        /// <summary>
        /// 根据类别获取公开博客帖子列表。
        /// </summary>
        /// <param name="category"></param>
        /// <param name="blogGet"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<Result> GetPostListByCategoryPublic([FromQuery] string category, [FromBody] BlogGetPublish blogGet)
        {
            return await blogService.GetPostListByCategoryPublicAsync(category, blogGet);
        }
        /// <summary>
        /// 根据标签获取公开博客帖子列表。
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="blogGet"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<Result> GetPostListByTagPublic([FromQuery] string tag, [FromBody] BlogGetPublish blogGet)
        {
            return await blogService.GetPostListByTagPublicAsync(tag, blogGet);
        }
        /// <summary>
        /// 获取已删除的博客帖子列表。
        /// </summary>
        /// <param name="blogGet"></param>
        /// <returns></returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> GetPostListDelete([FromBody] BlogGet blogGet)
        {
            return await blogService.GetPostListDeleteAsync(blogGet);
        }

    }
}