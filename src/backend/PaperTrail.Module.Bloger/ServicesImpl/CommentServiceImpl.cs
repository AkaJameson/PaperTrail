using PaperTrail.Module.Bloger.Models;
using PaperTrail.Module.Bloger.Services;
using PaperTrail.Storage.Entitys;
using Si.CoreHub.OperateResult;
using Si.EntityFramework.Extension.Abstraction;

namespace PaperTrail.Module.Bloger.ServicesImpl
{
    public class CommentServiceImpl : ICommentService
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly ICurrentUser _currentUser;
        public CommentServiceImpl(IUnitOfWork unitofWork, ICurrentUser currentUser)
        {
            _unitofWork = unitofWork;
            _currentUser = currentUser;
        }
        public async Task<Result> AuditComment(List<int> commentId)
        {
            var comments = await _unitofWork.GetRepository<Comment>().FindAsync(p => p.isAudit && commentId.Contains(p.Id));
            if (comments.Count() == 0)
            {
                return Result.Failed("没有需要审核的评论");
            }
            foreach (var item in comments)
            {
                item.isAudit = true;
            }
            await _unitofWork.GetRepository<Comment>().UpdateRangeAsync(comments);
            await _unitofWork.CommitAsync();
            return Result.Successed("审核成功");
        }

        public async Task<Result> CreateComment(CommentRequest commentRequest, string IP)
        {
            var comment = new Comment()
            {
                BlogId = commentRequest.BlogId,
                Content = commentRequest.Content,
                Name = commentRequest.Name,
                Email = commentRequest.Email,
                ParentId = commentRequest.ParentId.HasValue ? commentRequest.ParentId.Value : 0,
                CreateTime = DateTime.Now,
                Ip = IP,
                isAudit = false,
            };
            await _unitofWork.GetRepository<Comment>().AddAsync(comment);
            await _unitofWork.CommitAsync();
            return Result.Successed("评论成功");
        }

        public async Task<Result> DeleteComment(int commentId)
        {
            await _unitofWork.GetRepository<Comment>().DeleteAsync(commentId);
            return Result.Successed("删除成功");
        }

        public async Task<Result> UnAuditCommentList(int pageIndex = 1, int pageSize = 20)
        {
            var unAuditlist = await _unitofWork.GetRepository<Comment>()
                                                .GetPagedAsync(pageIndex, pageSize, p => p.isAudit == false, p => p.Id);
                                                
            var result = new List<object>();
            foreach (var item in unAuditlist.Items ?? new List<Comment>())
            {
                result.Add(new
                {
                    item.Id,
                    item.Name,
                    item.Email,
                    item.Ip,
                    item.Content,
                    item.CreateTime
                });
            }
            return Result.Successed(new{ result,total = unAuditlist.TotalCount}, "获取成功");
        }
    }
}
