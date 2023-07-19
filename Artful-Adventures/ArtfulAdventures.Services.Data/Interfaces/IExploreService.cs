namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Web.ViewModels;
using ArtfulAdventures.Web.ViewModels.HashTag;

public interface IExploreService
{
    Task<ExploreViewModel> GetExploreViewModelAsync(int page);

    Task<ExploreViewModel> SortByTagAsync(int[] tagsIds, int page);
}

