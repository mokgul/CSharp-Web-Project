namespace ArtfulAdventures.Web.ViewModels.Challenges;

public class ChallengesViewModel
{
    public ICollection<ChallengeVisualizeViewModel> Challenges { get; set; } = new HashSet<ChallengeVisualizeViewModel>();
}

