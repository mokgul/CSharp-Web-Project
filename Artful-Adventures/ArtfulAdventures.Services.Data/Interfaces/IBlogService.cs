namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Web.ViewModels.Blog;
using ArtfulAdventures.Web.ViewModels.Picture;

public interface IBlogService
{
    Task<BlogAddFormModel> GetBlogViewModelAsync();

    Task CreateBlogAsync(BlogAddFormModel model, string id, string? path);

    Task<BlogDetailsViewModel> GetBlogDetailsAsync(string id);

    Task<BlogVisualizeModel> GetAllBlogsAsync();
    

}

