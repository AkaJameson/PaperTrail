using PaperTrail.Model.Manager.Models;
using PaperTrail.Model.Manager.Services;
using PaperTrail.Storage.Entitys;
using Si.CoreHub.OperateResult;
using Si.EntityFramework.Extension.Abstraction;

namespace PaperTrail.Model.Manager.ServicesImpl
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly IUnitOfWork _unitofWork;

        public CategoryServiceImpl(IUnitOfWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public async Task<Result> AddCategory(CategoryRequest category)
        {
            if(await _unitofWork.GetRepository<Category>().ExistsAsync(p=>p.Name == category.CategoryName))
            {
                return Result.Successed("标签已存在");
            }
            await _unitofWork.GetRepository<Category>().AddAsync(new Category
            {
                Name = category.CategoryName,
                Description = category.Description
            });
            await _unitofWork.CommitAsync();
            return Result.Successed( "添加成功");
        }

        public async Task<Result> AllCategory()
        {
            var result = await _unitofWork.GetRepository<Category>().GetAllAsync();
            return Result.Successed<object>(new
            {
                data = result?.Select(p => new
                {
                    tag = p.Name,
                    descirption = p.Description,
                    createTime = p.CreatedTime
                }).ToList()
            });
        }

        public async Task<Result> UpdateCategory(UpdateCategoryRequest updateCategoryRequest)
        {
            var category = await _unitofWork.GetRepository<Category>().SingleOrDefaultAsync(p => p.Name == updateCategoryRequest.CategoryName);
            if(category == null)
            {
                return Result.Failed("分类不存在");
            }
            category.Name = updateCategoryRequest.newCategoryName;
            if (!string.IsNullOrEmpty(updateCategoryRequest.newDescription))
            {
                category.Description = updateCategoryRequest.newDescription;
            }
            await _unitofWork.GetRepository<Category>().UpdateAsync(category);
            return Result.Successed("更新成功");
        }
    }
}
