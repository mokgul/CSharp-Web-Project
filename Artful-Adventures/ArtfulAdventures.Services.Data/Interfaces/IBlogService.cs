namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Web.ViewModels.Blog;
using ArtfulAdventures.Web.ViewModels.Picture;

public interface IBlogService
{
    Task<BlogAddFormModel> GetBlogViewModelAsync();

    Task CreateBlogAsync(BlogAddFormModel model, string id, string? path);

    Task<BlogDetailsViewModel> GetBlogDetailsAsync(string id, string currentUser);

    Task<BlogVisualizeModel> GetAllBlogsAsync(string sort, int page);

    Task<BlogVisualizeModel> GetAllBlogsForManageAsync(string sort, string userId, int page);

    Task<BlogAddFormModel> GetBlogToEditAsync(string id);

    Task DeleteBlogAsync(string id, string userId);

    Task EditBlogAsync(BlogAddFormModel model, string id, string? path);

    Task LikeBlogAsync(string blogId);
}

