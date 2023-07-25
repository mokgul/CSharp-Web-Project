namespace ArtfulAdventures.Services.Data;

using System.Threading.Tasks;

using ArtfulAdventures.Data;
using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Services.Data.Interfaces;
using ArtfulAdventures.Web.ViewModels.Blog;
using ArtfulAdventures.Web.ViewModels.Comment;

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

    public async Task<BlogDetailsViewModel> GetBlogDetailsAsync(string id)
    {
        
        var blog = await _data.Blogs.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id.ToString() == id.ToLower());

        if(blog == null)
        {
            throw new NullReferenceException();
        }

        var user = await _data.Users.Include(b => b.Blogs).FirstOrDefaultAsync(x => x.Id == blog.AuthorId);

        var model = new BlogDetailsViewModel()
        {
            Id = blog.Id.ToString(),
            Title = blog.Title,
            Content = blog.Content,
            Author = user!.UserName,
            CreatedOn = blog.CreatedOn,
            Likes = blog.Likes,
            ImageUrl = Path.GetFileName(blog.ImageUrl),
            Comments = blog.Comments.Select(c => new CommentViewModel()
            {
                Author = c.Author,
                Content = c.Content,
                CreatedOn = c.CreatedOn,
            }).ToList(),
        };
        return model;
    }

    public async Task<BlogVisualizeModel> GetAllBlogsAsync()
    {
        var blogs = await _data.Blogs.Include(a => a.Author).OrderByDescending(x => x.CreatedOn).ToListAsync();
        blogs = blogs.OrderByDescending(x => x.CreatedOn).ToList();
        var model = new BlogVisualizeModel()
        {
            Blogs = blogs.Select(b => new BlogDetailsViewModel()
            {
                Id = b.Id.ToString(),
                Title = b.Title,
                Content = b.Content,
                Author = b.Author.UserName,
                CreatedOn = b.CreatedOn,
                Likes = b.Likes,
                ImageUrl = Path.GetFileName(b.ImageUrl),
            }).ToList(),
        }; 
        
        return model;
    }
}

