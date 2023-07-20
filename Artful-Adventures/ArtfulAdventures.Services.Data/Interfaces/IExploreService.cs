namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Web.ViewModels;

public interface IExploreService
{
    Task<ExploreViewModel> GetExploreViewModelAsync(int page);

    Task<ExploreViewModel> SortByTagAsync(int[] tagsIds, int page);
}

