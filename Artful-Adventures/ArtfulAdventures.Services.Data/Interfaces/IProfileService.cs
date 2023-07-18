namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Web.ViewModels.UserProfile;

public interface IProfileService
{
    Task<ProfileViewModel?> GetProfileViewModelAsync(string username, string userId);

    Task<string> FollowAsync(string username, string userId);

    Task UnfollowAsync(string username, string userId);

    Task<FollowViewModel?> GetFollowersAsync(string username);

    Task<FollowViewModel?> GetFollowingAsync(string username);

    Task<PortfolioViewModel?> GetPortfolioAsync(string username);

    Task<PortfolioViewModel?> GetCollectionAsync(string username);
}

