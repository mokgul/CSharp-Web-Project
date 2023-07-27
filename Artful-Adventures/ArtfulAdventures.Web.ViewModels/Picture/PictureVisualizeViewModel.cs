namespace ArtfulAdventures.Web.ViewModels.Picture;

using System.ComponentModel;

public class PictureVisualizeViewModel
{
    public string Id { get; set; } = null!;

    /// <summary>
    /// "This is a property used for sorting purposes only, when the users wants to display picture by specific user!"
    /// </summary>
    public string Owner { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public int Likes { get; set; }

    public string PictureUrl { get; set; } = null!;

    public List<string> HashTags { get; set; } = new List<string>();
}

