namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Web.ViewModels.Challenges;

public interface IChallengeService
{
    Task<ChallengesViewModel> GetAllAsync(int page);

    Task<ChallengeDetailsViewModel> GetChallengeDetailsAsync(int id);

    Task ParticipateAsync(int id, string userId, string path);
}

