namespace ArtfulAdventures.Services.Data;

using ArtfulAdventures.Web.ViewModels.Picture;

public static class FilterBrokenUrls
{
    public static async Task<ICollection<PictureVisualizeViewModel>> FilterAsync(ICollection<PictureVisualizeViewModel> pictures)
    {
        string[] files = Directory.GetFiles(@"wwwroot\images");
        files = files.Select(x => Path.GetFileName(x)).ToArray();
        var picturesFiltered = new List<PictureVisualizeViewModel>();
        foreach(var picture in pictures)
        {
            if (files.Contains(picture.PictureUrl))
            {
                picturesFiltered.Add(picture);
            }
        }
        return picturesFiltered;
    }
}

