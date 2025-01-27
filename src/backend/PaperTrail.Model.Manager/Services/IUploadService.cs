using Microsoft.AspNetCore.Http;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Model.Manager.Services
{
    public interface IUploadService
    {
        Task<Result> UploadImage(IFormFile file, HttpRequest request);
    }
}
