using PaperTrail.Module.Bloger.Models;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Module.Bloger.Services
{
    public interface ICategoryService
    {
        Task<Result> AddCategory(CategoryRequest category);
        Task<Result> AllCategory();
        Task<Result> UpdateCategory(UpdateCategoryRequest updateCategoryRequest);
    }
}
