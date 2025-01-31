using PaperTrail.Module.Bloger.Models;
using PaperTrail.Module.Bloger.Services;
using PaperTrail.Storage.Entitys;
using Si.CoreHub.OperateResult;
using Si.EntityFramework.Extension.Abstraction;

namespace PaperTrail.Module.Bloger.ServicesImpl
{
    public class TagServiceImpl : ITagService
    {
        private readonly IUnitOfWork _unitofWork;

        public TagServiceImpl(IUnitOfWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<Result> AddTag(TagRequest tag)
        {
            if (await _unitofWork.GetRepository<Tag>().ExistsAsync(p => p.Name == tag.TagName))
            {
                return Result.Failed("标签已存在");
            }
            await _unitofWork.GetRepository<Tag>().AddAsync(new Tag
            {
                Name = tag.TagName,
                Description = tag.Description,
            });
            await _unitofWork.CommitAsync();
            return Result.Successed("添加成功");
        }

        public async Task<Result> AllTags()
        {
            var result = await _unitofWork.GetRepository<Tag>().GetAllAsync();
            return Result.Successed<object>(new
            {
                data = result?.Select(p => new
                {
                    tag = p.Name,
                    descirption = p.Description,
                    CreatedTime = p.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToList()
            });
        }

        public async Task<Result> UpdateTag(UpdateTagRequest updateTagRequest)
        {
            if (!await _unitofWork.GetRepository<Tag>().ExistsAsync(p => p.Name == updateTagRequest.TagName))
            {
                return Result.Failed("标签不存在");
            }
            var tag = await _unitofWork.GetRepository<Tag>().SingleOrDefaultAsync(p => p.Name == updateTagRequest.TagName);
            tag.Name = updateTagRequest.newTagName;
            if (!string.IsNullOrEmpty(updateTagRequest.newDescription))
            {
                tag.Description = updateTagRequest.newDescription;
            }
            await _unitofWork.GetRepository<Tag>().UpdateAsync(tag);
            await _unitofWork.CommitAsync();
            return Result.Successed("更新成功");
        }
    }
}
