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
    public class TagController : DefaultController
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<Result> AllTags()
        {
            return await _tagService.AllTags();
        }
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="tagRequest"></param>
        /// <returns></returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> AddTag([FromBody] TagRequest tagRequest)
        {
            return await _tagService.AddTag(tagRequest);
        }
        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="updateTagRequest"></param>
        /// <returns></returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> UpdateTag([FromBody] UpdateTagRequest updateTagRequest)
        {
            return await _tagService.UpdateTag(updateTagRequest);
        }
    }
}
