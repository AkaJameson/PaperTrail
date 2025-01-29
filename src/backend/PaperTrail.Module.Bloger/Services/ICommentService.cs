using PaperTrail.Module.Bloger.Models;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Module.Bloger.Services
{
    public interface ICommentService
    {
        /// <summary>
        /// 获取未审核的评论列表
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示的评论数量</param>
        /// <returns>返回未审核评论的查询结果</returns
        Task<Result> UnAuditCommentList(int pageIndex = 1, int pageSize = 20);
        /// <summary>
        /// 审核评论
        /// </summary>
        /// <param name="commentId">待审核的评论ID列表</param>
        /// <returns>返回审核结果</returns>
        Task<Result> AuditComment(List<int> commentId);
        /// <summary>
        /// 创建新评论
        /// </summary>
        /// <param name="commentRequest">评论请求对象，包含评论的相关信息</param>
        /// <returns>返回创建评论的结果</returns>
        Task<Result> CreateComment(CommentRequest commentRequest,string ip);
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="commentId">待删除的评论ID</param>
        /// <returns>返回删除评论的结果</returns>
        Task<Result> DeleteComment(int commentId);
    }
}
