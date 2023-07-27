namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Web.ViewModels;

public interface IFollowingService
{
    Task<ExploreViewModel> GetExploreViewModelAsync(string sort, int page, string username);
}

