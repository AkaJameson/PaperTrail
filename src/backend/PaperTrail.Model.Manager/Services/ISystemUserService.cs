using Microsoft.AspNetCore.Http;
using PaperTrail.Model.Manager.Models;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Model.Manager.Services
{
    public interface ISystemUserService
    {
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Result> UpdatePassword(int userId,UpdateUserPasswordRequest request);
        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="avaterPath"></param>
        /// <returns></returns>
        Task<Result> UploadAvater(int userId,IFormFile file, HttpRequest request);
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        Task<Result> EditUserInfo(int userId, EditUserInfo userInfo);
        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result> UserDetail(int userId);
    }
}
