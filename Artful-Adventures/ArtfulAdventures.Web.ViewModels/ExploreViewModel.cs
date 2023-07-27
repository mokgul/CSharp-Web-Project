namespace ArtfulAdventures.Web.ViewModels;

using ArtfulAdventures.Web.ViewModels.HashTag;
using ArtfulAdventures.Web.ViewModels.Picture;

using System.Drawing;

public class ExploreViewModel
{

    public ICollection<PictureVisualizeViewModel> Pictures { get; set; }

    public List<HashTagViewModel> HashTags { get; set; } = new List<HashTagViewModel>();

    public List<HashTagViewModel> TagsForDropDown { get; set; } = new List<HashTagViewModel>();
}

