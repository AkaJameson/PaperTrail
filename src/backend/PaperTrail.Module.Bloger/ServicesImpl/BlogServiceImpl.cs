using Microsoft.EntityFrameworkCore;
using PaperTrail.Module.Bloger.Models;
using PaperTrail.Module.Bloger.Services;
using PaperTrail.Storage.Entitys;
using Si.CoreHub.OperateResult;
using Si.EntityFramework.Extension.Abstraction;

namespace PaperTrail.Module.Bloger.ServicesImpl
{
    public class BlogServiceImpl : IBlogService
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly ICurrentUser currentUser;
        public BlogServiceImpl(IUnitOfWork unitofWork, ICurrentUser currentUser)
        {
            _unitofWork = unitofWork;
            this.currentUser = currentUser;
        }

        public async Task<Result> CreatePostAsync(BlogCreate post)
        {
            var user = await _unitofWork.GetRepository<User>().SingleOrDefaultAsync(x => x.Id == currentUser.UserId);
            if (user == null)
                return Result.Failed("用户不存在");
            var tags = (await _unitofWork.GetRepository<Tag>().FindAsync(x => post.tagId.Contains(x.Id))).ToList();
            var categorys = (await _unitofWork.GetRepository<Category>().FindAsync(x => post.categoryId.Contains(x.Id))).ToList();
            var newBlog = new Blog
            {
                UserId = user.Id,
                Title = post.Title,
                Content = post.Content,
                Tags = tags,
                Categorys = categorys,
                IsPublish = post.isPublish
            };
            user.Blogs.Add(newBlog);
            await _unitofWork.CommitAsync();
            return Result.Successed("添加成功");
        }

        public async Task<Result> DeletePostAsync(int id)
        {
            var blog = await _unitofWork.GetRepository<Blog>().SingleOrDefaultAsync(x => x.Id == id && x.UserId == currentUser.UserId);
            if (blog == null)
            {
                return Result.Failed("文章不存在");
            }
            await _unitofWork.GetRepository<Blog>().SoftDeleteAsync(blog);
            await _unitofWork.CommitAsync();
            return Result.Successed("删除成功");
        }
        public async Task<Result> GetPostAsync(int id)
        {
            var blog = await _unitofWork.GetRepository<Blog>().SingleOrDefaultAsync(x => x.Id == id && x.UserId == currentUser.UserId);
            if (blog == null)
                return Result.Failed("文章不存在");
            var comments = blog.Comments ?? new List<Comment>();
            var commentTree = BuildCommentTree(comments);
            return Result.Successed(new
            {
                blog = new
                {
                    categorys = blog.Categorys.Select(p => new
                    {
                        p.Name,
                        p.Description,
                        p.CreatedTime,
                    }),
                    tags = blog.Tags.Select(p => new
                    {
                        p.Name,
                        p.Description,
                        p.CreatedTime
                    }),
                    title = blog.Title,
                    content = blog.Content,
                    createTime = blog.CreatedTime,
                    ispublish = blog.IsPublish,
                },
                comments = commentTree
            });
        }

        public async Task<Result> GetPostListAsync(BlogGet blogGet)
        {
            var blogList = (await _unitofWork.GetRepository<Blog>().GetPagedAsync(blogGet.PageIndex!.Value, blogGet.PageSize!.Value, p => p.IsPublish == blogGet.isPublish
                                                                                && p.IsDeleted == false, p => p.Id, false));
            var result = new List<object>();
            foreach (var item in blogList.Items?.ToList() ?? new List<Blog>())
            {
                result.Add(new
                {
                    id = item.Id,
                    title = item.Title,
                    content = item.Content.Substring(0, 200) + "...",
                    publishTime = item.CreatedTime,
                    tags = item.Tags.Select(p => p.Name).ToList(),
                    category = item.Categorys.Select(p => p.Name).ToList(),
                });
            }
            return Result.Successed(new { result, total = blogList.TotalCount }, "获取成功");
        }

        public async Task<Result> GetPostListByTagAsync(string tag, BlogGet blogGet)
        {
            var blogs = (await _unitofWork.GetRepository<Tag>().Query().SingleOrDefaultAsync(p => p.Name == tag))?.Blogs;
            if (blogs == null)
            {
                return Result.Failed("该标签下没有文章");
            }
            var blogList = blogs.Skip((blogGet.PageIndex.Value - 1) * blogGet.PageSize.Value).Take(blogGet.PageSize.Value).ToList();
            var result = new List<object>();
            foreach (var item in blogList?.ToList() ?? new List<Blog>())
            {
                result.Add(new
                {
                    id = item.Id,
                    title = item.Title,
                    content = item.Content.Substring(0, 200) + "...",
                    publishTime = item.CreatedTime,
                    tags = item.Tags.Select(p => p.Name).ToList(),
                    category = item.Categorys.Select(p => p.Name).ToList(),
                });
            }
            return Result.Successed(result, "获取成功");
        }
        public async Task<Result> GetPostListByCategoryAsync(string category, BlogGet blogGet)
        {
            var blogs = (await _unitofWork.GetRepository<Category>().Query().SingleOrDefaultAsync(p => p.Name == category))?.Blogs;
            if (blogs == null)
            {
                return Result.Failed("该标签下没有文章");
            }
            var blogList = blogs.Skip((blogGet.PageIndex.Value - 1) * blogGet.PageSize.Value).Take(blogGet.PageSize.Value).ToList();
            var result = new List<object>();
            foreach (var item in blogList?.ToList() ?? new List<Blog>())
            {
                result.Add(new
                {
                    id = item.Id,
                    title = item.Title,
                    content = item.Content.Substring(0, 200) + "...",
                    publishTime = item.CreatedTime,
                    tags = item.Tags.Select(p => p.Name).ToList(),
                    category = item.Categorys.Select(p => p.Name).ToList(),
                });
            }
            return Result.Successed(result, "获取成功");
        }
        public async Task<Result> UpdatePostAsync(BlogUpdate post)
        {
            var blog = await _unitofWork.GetRepository<Blog>().SingleOrDefaultAsync(x => x.Id == post.Id);
            if (blog == null)
            {
                return Result.Failed("文章不存在");
            }
            if (!string.IsNullOrEmpty(post.Title))
            {
                blog.Title = post.Title;
            }
            if (!string.IsNullOrEmpty(post.Content))
            {
                blog.Content = post.Content;
            }
            if (post.Tags != null)
            {
                var tags = (await _unitofWork.GetRepository<Tag>().FindAsync(p => post.Tags.Contains(p.Name))).ToList();
                blog.Tags = tags;
            }
            if (post.Categorys != null)
            {
                var categories = (await _unitofWork.GetRepository<Category>().FindAsync(p => post.Categorys.Contains(p.Name))).ToList();
                blog.Categorys = categories;
            }
            await _unitofWork.GetRepository<Blog>().UpdateAsync(blog);
            await _unitofWork.CommitAsync();
            return Result.Successed("更新成功");
        }
        // 构建评论树形结构，选择所需的字段
        private List<object> BuildCommentTree(IEnumerable<Comment> comments)
        {
            comments = comments.Where(p => p.isAudit == true);
            var topLevelComments = comments.Where(c => c.ParentId == 0).ToList();
            var commentTree = new List<object>();

            foreach (var comment in topLevelComments)
            {
                // 为每个顶级评论添加子评论
                var treeItem = new
                {
                    comment.Id,
                    comment.Content,
                    comment.Name,
                    comment.Email,
                    comment.Like,
                    comment.CreateTime,
                    comment.Ip,
                    Childrens = BuildChildComments(comment, comments)
                };
                commentTree.Add(treeItem);
            }

            return commentTree;
        }
        // 递归构建子评论
        private List<object> BuildChildComments(Comment parentComment, IEnumerable<Comment> allComments)
        {
            var children = allComments.Where(c => c.ParentId == parentComment.Id).ToList();
            var childTree = new List<object>();

            foreach (var child in children)
            {
                var childItem = new
                {
                    child.Id,
                    child.Content,
                    child.Name,
                    child.Email,
                    child.Like,
                    child.CreateTime,
                    child.Ip,
                    Childrens = BuildChildComments(child, allComments)
                };

                childTree.Add(childItem);
            }
            return childTree;
        }


    }
}
