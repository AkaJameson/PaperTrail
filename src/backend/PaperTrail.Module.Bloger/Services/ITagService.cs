using PaperTrail.Module.Bloger.Models;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Module.Bloger.Services
{
    /// <summary>
    /// 标签服务
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        Task<Result> AllTags();
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task<Result> AddTag(TagRequest tag);
        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="updateTagRequest"></param>
        /// <returns></returns>
        Task<Result> UpdateTag(UpdateTagRequest updateTagRequest);
    }
}
