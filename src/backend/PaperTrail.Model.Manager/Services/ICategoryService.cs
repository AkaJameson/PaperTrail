using PaperTrail.Model.Manager.Models;
using Si.CoreHub.OperateResult;

namespace PaperTrail.Model.Manager.Services
{
    public interface ICategoryService
    {
        Task<Result> AddCategory(CategoryRequest category);
        Task<Result> AllCategory();
        Task<Result> UpdateCategory(UpdateCategoryRequest updateCategoryRequest);
    }
}
