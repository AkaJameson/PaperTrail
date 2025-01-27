using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PaperTrail.Model.Manager.Models;
using PaperTrail.Model.Manager.Services;
using PaperTrail.Storage.Entitys;
using PaperTrail.Utilities;
using Si.CoreHub.OperateResult;
using Si.EntityFramework.Extension.Abstraction;

namespace PaperTrail.Model.Manager.ServicesImpl
{
    public class SystemUserServiceImpl : ISystemUserService
    {

        private readonly IUnitOfWork _unitofWork;
        private readonly IConfiguration configuration;
        public SystemUserServiceImpl(IUnitOfWork unitofWork, IConfiguration configuration)
        {
            _unitofWork = unitofWork;
            this.configuration = configuration;
        }

        public async Task<Result> EditUserInfo(int userId, EditUserInfo userInfo)
        {
            var user = await _unitofWork.GetRepository<User>().GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Failed("用户不存在");
            }
            if (userInfo.QQ != null)
            {
                user.QQ = userInfo.QQ;
            }
            if (userInfo.Github != null)
            {
                user.Github = userInfo.Github;
            }
            if (userInfo.Name != null)
            {
                user.Name = userInfo.Name;
            }
            if (userInfo.Email != null)
            {
                user.Email = userInfo.Email;
            }
            await _unitofWork.CommitAsync();
            return Result.Successed("更新成功");
        }

        public async Task<Result> UpdatePassword(int userId, UpdateUserPasswordRequest request)
        {
            var user = await _unitofWork.GetRepository<User>().GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Failed("用户不存在");
            }
            var key = configuration.GetValue<string>("AesConfig:Key");
            var iv = configuration.GetValue<string>("AesConfig:IV");
            var newPasswordHash = StableAesCrypto.Encrypt(request.newPassword, key, iv);
            if (user.PasswordHash == newPasswordHash)
            {
                return Result.Failed("新密码不能与旧密码相同");
            }
            if (user.PasswordHash == StableAesCrypto.Encrypt(request.oldPassword, key, iv))
            {
                user.PasswordHash = newPasswordHash;
                await _unitofWork.CommitAsync();
                return Result.Successed("更新成功");
            }
            else
            {
                return Result.Failed("旧密码错误");
            }
        }

        public async Task<Result> UploadAvater(int userId, IFormFile file, HttpRequest request)
        {
            var user = await _unitofWork.GetRepository<User>().GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Failed("用户不存在");
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                return Result.Failed("仅支持 JPG/PNG/JEPG 格式");

            // 生成唯一文件名
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            var uploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            user.AvatarPath = $"uploads/{fileName}";
            await _unitofWork.GetRepository<User>().UpdateAsync(user);
            await _unitofWork.CommitAsync();
            return Result.Successed<object>(new
            {
                url = $"{request.Scheme}://{request.Host}/uploads/{fileName}"
            });
        }

        public async Task<Result> UserDetail(int userId)
        {
            var user = await _unitofWork.GetRepository<User>().GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Failed("用户不存在");
            }
            return Result.Successed<object>(new
            {
                userName = user.Name,
                qq = user.QQ ?? string.Empty,
                github = user.Github ?? string.Empty,
                email = user.Email,
                avatar = user.AvatarPath ?? string.Empty,
                blogCount = user.Blogs.Count,
                essayCount = user.Essays.Count,
            });
        }
    }
}
