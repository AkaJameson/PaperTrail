using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperTrail.Model.Manager.Models;
using PaperTrail.Model.Manager.Services;
using PaperTrail.Storage.Enums;
using Si.CoreHub;
using Si.CoreHub.OperateResult;
using Si.EntityFramework.PermGuard.Handlers;

namespace PaperTrail.Model.Manager.Controllers
{
    [ApiController]
    public class CategoryController : DefaultController
    {
        private ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<Result> AllCategorys() => await _categoryService.AllCategory();
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="categoryRequest"></param>
        /// <returns></returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> AddCategory([FromBody] CategoryRequest categoryRequest)=> await _categoryService.AddCategory(categoryRequest);
        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="updateCategoryRequest"></param>
        /// <returns></returns>
        [Permission(PermissionConst.Write)]
        [HttpPost]
        public async Task<Result> UpdateCategory([FromBody] UpdateCategoryRequest updateCategoryRequest) => await _categoryService.UpdateCategory(updateCategoryRequest);
    }
}
