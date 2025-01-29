using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaperTrail.Module.Bloger.Services;
using PaperTrail.Storage.Enums;
using Si.CoreHub;
using Si.CoreHub.OperateResult;
using Si.EntityFramework.PermGuard.Handlers;

namespace PaperTrail.Module.Bloger.Controllers
{
    [ApiController]
    public class UploadController : DefaultController
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> UploadImage(IFormFile file)
        {
            return await _uploadService.UploadImage(file, HttpContext.Request);
        }
    }
}
