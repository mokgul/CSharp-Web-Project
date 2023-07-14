namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Web.ViewModels;

public interface IExploreService
{
    Task<ExploreViewModel> GetExploreViewModelAsync(int page);
}

