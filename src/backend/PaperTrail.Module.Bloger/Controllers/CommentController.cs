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
    [ApiController]
    public class CommentController : DefaultController
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }
        [Permission(PermissionConst.Write)]
        [HttpGet]
        public Task<Result> UnAuditCommentList([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            return commentService.UnAuditCommentList(pageIndex.HasValue ? pageIndex.Value : 1,
                                                     pageSize.HasValue ? pageSize.Value : 20);
        }
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public Task<Result> AuditComment([FromBody] List<int> commentIds)
        {
            return commentService.AuditComment(commentIds);
        }
        [AllowAnonymous]
        [HttpPost]
        public Task<Result> CreateComment([FromBody] CommentRequest commentRequest)
        {
            return commentService.CreateComment(commentRequest, HttpContext.Connection.RemoteIpAddress.ToString());
        }
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public Task<Result> DeleteComment(int commentId)
        {
            return commentService.DeleteComment(commentId);
        }

    }
}
