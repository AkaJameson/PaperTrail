using Microsoft.AspNetCore.Http;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Module.Bloger.Services
{
    public interface IUploadService
    {
        Task<Result> UploadImage(IFormFile file, HttpRequest request);
    }
}
