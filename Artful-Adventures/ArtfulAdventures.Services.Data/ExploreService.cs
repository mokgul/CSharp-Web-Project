namespace ArtfulAdventures.Services.Data;

using System.Threading.Tasks;
using System.Xml.Linq;

using ArtfulAdventures.Data;
using ArtfulAdventures.Services.Data.Interfaces;
using ArtfulAdventures.Web.ViewModels;
using ArtfulAdventures.Web.ViewModels.HashTag;
using ArtfulAdventures.Web.ViewModels.Picture;

using Microsoft.EntityFrameworkCore;

public class ExploreService : IExploreService
{
    private readonly ArtfulAdventuresDbContext _data;

    public ExploreService(ArtfulAdventuresDbContext data)
    {
        _data = data;
    }
    public async Task<ExploreViewModel> GetExploreViewModelAsync()
    {
        var hashtags = await _data.HashTags.Select(h => new HashTagViewModel()
        {
            Id = h.Id,
            Name = h.Type
        }).ToListAsync();

        var urls = await _data.Pictures.Select(p => new PictureVisualizeViewModel()
        {
            PictureUrl = p.Url,
        }).ToArrayAsync();

        var ftpRemotePaths = new List<string>();
        foreach (var url in urls)
        {
            var path = Path.GetFileName(url.PictureUrl);
            if (!ftpRemotePaths.Contains(path))
                ftpRemotePaths.Add(path);
        }
        ExploreViewModel model = new ExploreViewModel()
        {
            HashTags = hashtags,
            PicturesIds = ftpRemotePaths.ToArray().Select(p => new PictureVisualizeViewModel()
            {
                PictureUrl = p
            }).ToArray(),
        };
        return model;
    }
}

