namespace ArtfulAdventures.Web.ViewModels.Picture;


public class PictureVisualizeViewModel
{
    public string Id { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public int Likes { get; set; }

    public string PictureUrl { get; set; } = null!;
}

