using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaperTrail.Module.Bloger.Models;
using PaperTrail.Module.Bloger.Services;
using PaperTrail.Storage.Enums;
using Si.CoreHub;
using Si.CoreHub.OperateResult;
using Si.EntityFramework.Extension.Abstraction;
using Si.EntityFramework.Extension.Rbac.Handlers;

namespace PaperTrail.Module.Bloger.Controllers
{
    [ApiController]
    public class SystemUserController : DefaultController
    {
        private readonly IUserInfo currentUser;
        private readonly IUserService systemUserService;
        public SystemUserController(IUserService systemUserService, IUserInfo currentUser = null)
        {
            this.currentUser = currentUser;
            this.systemUserService = systemUserService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<Result> UserDetail(int userId)
        {
            return await systemUserService.UserDetail(userId);
        }
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> UpdatePassword([FromBody] UpdateUserPasswordRequest request)
        {
            if (currentUser == null || currentUser.UserId == null)
            {
                return Result.Failed("用户不存在");
            };
            if (request.oldPassword != request.confirmPassword)
            {
                return Result.Failed("旧密码和确认密码不一致");
            }
            return await systemUserService.UpdatePassword(currentUser.UserId, request);
        }

        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> UpdateUserInfo([FromBody] EditUserInfo request)
        {
            if (currentUser == null || currentUser.UserId == null)
            {
                return Result.Failed("用户不存在");
            };
            return await systemUserService.EditUserInfo(currentUser.UserId, request);
        }
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> UploadAvator(IFormFile file)
        {
            if (currentUser == null || currentUser.UserId == null)
            {
                return Result.Failed("用户不存在");
            }
            if (file == null)
            {
                return Result.Failed("文件为空");
            }
            return await systemUserService.UploadAvater(currentUser.UserId, file, HttpContext.Request);
        }

    }
}
