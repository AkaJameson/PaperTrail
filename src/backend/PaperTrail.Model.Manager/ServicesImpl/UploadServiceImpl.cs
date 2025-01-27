using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaperTrail.Model.Manager.Services;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Model.Manager.ServicesImpl
{
    public class UploadServiceImpl : IUploadService
    {
        public async Task<Result> UploadImage(IFormFile file,HttpRequest request)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                return Result.Failed("仅支持 JPG/PNG/GIF/WEBP 格式");

            // 生成唯一文件名
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            var uploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"..", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Result.Successed<object>(new
            {
                url = $"{request.Scheme}://{request.Host}/uploads/{fileName}"
            });
        }
    }
}
