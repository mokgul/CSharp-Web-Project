using ArtfulAdventures.Services.Common;

namespace ArtfulAdventures.Services.Data;

using System.Threading.Tasks;
using System.Xml.Linq;

using ArtfulAdventures.Data;
using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Services.Data.Interfaces;
using ArtfulAdventures.Web.ViewModels.Blog;
using ArtfulAdventures.Web.ViewModels.Comment;
using ArtfulAdventures.Web.ViewModels.Picture;

using Microsoft.EntityFrameworkCore;

public class BlogService : IBlogService
{
    private readonly ArtfulAdventuresDbContext _data;

    public BlogService(ArtfulAdventuresDbContext data)
    {
        _data = data;
    }

    public async Task CreateBlogAsync(BlogAddFormModel model, string id, string? path)
    {
        var user = await GetUser(id);
        Blog blog = new Blog
        {
            Title = model.Title,
            Content = model.Content,
            Author = user!,
            AuthorId = user!.Id,
            CreatedOn = DateTime.UtcNow,
            Likes = 0,
            ImageUrl = path,
        };
        await _data.Blogs.AddAsync(blog);
        await _data.SaveChangesAsync();
    }

    private async Task<ApplicationUser?> GetUser(string userId)
    {
        return await _data.Users.Include(m => m.Blogs).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
    }

    public async Task<BlogAddFormModel> GetBlogViewModelAsync()
    {
        var model = new BlogAddFormModel();
        return model;
    }

    public async Task<BlogDetailsViewModel> GetBlogDetailsAsync(string id, string currentUser)
    {
        
        var blog = await _data.Blogs.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id.ToString() == id.ToLower());

        if(blog == null)
        {
            throw new NullReferenceException();
        }

        var user = await _data.Users.Include(b => b.Blogs).FirstOrDefaultAsync(x => x.Id == blog.AuthorId);

        var userCommenting = await _data.Users.FirstOrDefaultAsync(x => x.Id.ToString() == currentUser);
        var userMute = userCommenting!.MuteUntil != null && userCommenting.MuteUntil > DateTime.UtcNow;
        var model = new BlogDetailsViewModel()
        {
            Id = blog.Id.ToString(),
            Title = blog.Title,
            Content = blog.Content,
            Author = user!.UserName,
            CreatedOn = blog.CreatedOn,
            Likes = blog.Likes,
            ImageUrl = Path.GetFileName(blog.ImageUrl),
            isCurrentUserMuted = userMute,
            CommentsCount = blog.Comments.Count,
            Comments = blog.Comments.Select(c => new CommentViewModel()
            {
                Id = c.Id,
                Author = c.Author,
                Content = c.Content,
                CreatedOn = c.CreatedOn,
            }).ToList()
        };
            foreach (var comment in model.Comments)
        {
            comment.AuthorPictureUrl = Path.GetFileName(_data.Users.FirstOrDefault(u => u.UserName == comment.Author).Url);
        }
        return model;
    }

    public async Task<BlogVisualizeModel> GetAllBlogsAsync(string sort, int page = 1)
    {
        if (!ValidatePage.Validate(page))
        {
            throw new ArgumentException("Invalid page number.");
        }
        int pageSize = 6;
        int skip = (page - 1) * pageSize;
        var blogs = await _data.Blogs.Include(a => a.Author).OrderByDescending(x => x.CreatedOn).ToListAsync();
        blogs = blogs.OrderByDescending(x => x.CreatedOn).ToList();
        var modelBlogs = blogs.Select(b => new BlogDetailsViewModel()
        {
            Id = b.Id.ToString(),
            Title = b.Title,
            Content = b.Content,
            Author = b.Author.UserName,
            CreatedOn = b.CreatedOn,
            Likes = b.Likes,
            ImageUrl = Path.GetFileName(b.ImageUrl),
        }).ToList();
        modelBlogs = await SortBlogsAsync(sort, modelBlogs);
        var model = new BlogVisualizeModel()
        {
            Blogs = modelBlogs.Skip(skip).Take(pageSize).ToList(),
        };
        
        return model;
    }

    public async Task LikeBlogAsync(string blogId)
    {
        var blog = await _data.Blogs.FirstOrDefaultAsync(x => x.Id.ToString() == blogId);
        if (blog == null)
        {
            throw new ArgumentException("Blog not found.");
        }
        blog.Likes++;
        await _data.SaveChangesAsync();
    }

    private async Task<List<BlogDetailsViewModel>> SortBlogsAsync(string sort, List<BlogDetailsViewModel> blogs)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return blogs;
        }
        var sortValidator = new ValidateSortParameter(_data);
        bool isValid = await sortValidator.Validate(sort);
        if (!isValid)
        {
            throw new ArgumentException("Invalid sort parameter!");
        }
        var owner = string.Empty;
        if (sort != "likes" && sort != "newest" && sort != "oldest")
        {
            owner = sort;
            sort = "author";
            if (!_data.Users.Any(u => u.UserName == owner))
                throw new ArgumentException($"User {owner} does not exist.");

            if (blogs.Count(p => p.Author == owner) == 0)
                throw new ArgumentException($"{owner} has not published any blogs yet.");
        }

        switch (sort)
        {
            case "likes":
                blogs = blogs.OrderByDescending(p => p.Likes).ToList();
                break;
            case "newest":
                blogs = blogs.OrderByDescending(p => p.CreatedOn).ToList();
                break;
            case "oldest":
                blogs = blogs.OrderBy(p => p.CreatedOn).ToList();
                break;
            case "author":
                blogs = blogs.Where(p => p.Author == owner).ToList();
                break;
            default:
                break;
        }
        return blogs;
    }

    public async Task<BlogAddFormModel> GetBlogToEditAsync(string id)
    {
        var blog = _data.Blogs.Include(u => u.Author).FirstOrDefault(x => x.Id.ToString() == id);
        if (blog == null)
        {
            throw new NullReferenceException("Blog not found.");
        }
        
        var model = new BlogAddFormModel()
        {
            Id = id,
            Title = blog.Title,
            Content = blog.Content,
            ImageUrl = Path.GetFileName(blog.ImageUrl),
        };
        return model;
    }

    public async Task<BlogVisualizeModel> GetAllBlogsForManageAsync(string sort, string userId, int page = 1)
    {
        if (!ValidatePage.Validate(page))
        {
            throw new ArgumentException();
        }
        int pageSize = 6;
        int skip = (page - 1) * pageSize;
        var blogs = await _data.Blogs.Include(a => a.Author).Where(b => b.AuthorId.ToString() == userId).OrderByDescending(x => x.CreatedOn).ToListAsync();
        blogs = blogs.OrderByDescending(x => x.CreatedOn).ToList();
        var modelBlogs = blogs.Select(b => new BlogDetailsViewModel()
        {
            Id = b.Id.ToString(),
            Title = b.Title,
            Content = b.Content,
            Author = b.Author.UserName,
            CreatedOn = b.CreatedOn,
            Likes = b.Likes,
            ImageUrl = Path.GetFileName(b.ImageUrl),
        }).ToList();
        modelBlogs = await SortBlogsAsync(sort, modelBlogs);
        var model = new BlogVisualizeModel()
        {
            Blogs = modelBlogs.Skip(skip).Take(pageSize).ToList(),
        };

        return model;
    }

    public async Task EditBlogAsync(BlogAddFormModel model, string id, string? path)
    {
        var blog = await _data.Blogs.FirstOrDefaultAsync(x => x.Id.ToString() == id)!;
        blog!.Title = model.Title;
        blog.Content = model.Content;
        if (!string.IsNullOrEmpty(path))
        {
            blog.ImageUrl = path;
        }
        _data.Blogs.Update(blog);
        await _data.SaveChangesAsync();
    }

    public async Task DeleteBlogAsync(string id, string userId)
    {
        var blog = await _data.Blogs.Include(c => c.Comments).Where(u => u.AuthorId.ToString() == userId).FirstOrDefaultAsync(x => x.Id.ToString() == id);
        blog!.Comments.Clear();
        _data.Blogs.Remove(blog!);
        _data.SaveChanges();
    }
}

