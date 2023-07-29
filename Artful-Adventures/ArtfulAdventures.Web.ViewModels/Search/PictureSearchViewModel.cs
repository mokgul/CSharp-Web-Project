namespace ArtfulAdventures.Web.ViewModels.Search;

public class PictureSearchViewModel
{
    public string Id { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string PictureUrl { get; set; } = null!;

    public string Owner { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
}

