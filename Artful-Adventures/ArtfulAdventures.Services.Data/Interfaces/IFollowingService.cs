namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Web.ViewModels;

public interface IFollowingService
{
    Task<ExploreViewModel> GetExploreViewModelAsync(int page, string username);

    Task<ExploreViewModel> SortByTagAsync(int[] tagsIds, int page, string username);
}

