namespace ArtfulAdventures.Services.Data;

using System.Threading.Tasks;
using System.Xml.Linq;

using ArtfulAdventures.Data;
using ArtfulAdventures.Services.Data.Interfaces;
using ArtfulAdventures.Web.ViewModels;
using ArtfulAdventures.Web.ViewModels.HashTag;
using ArtfulAdventures.Web.ViewModels.Picture;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class FollowingService : IFollowingService
{
    private readonly ArtfulAdventuresDbContext _data;

    public FollowingService(ArtfulAdventuresDbContext data)
    {
        _data = data;
    }
    public async Task<ExploreViewModel> GetExploreViewModelAsync(int page, string username)
    {
        int pageSize = 20;
        int skip = (page - 1) * pageSize;
        var user = await _data.Users.Include(f => f.Following).FirstOrDefaultAsync(u => u.UserName == username);

        var hashtags = await _data.HashTags.Select(h => new HashTagViewModel()
        {
            Id = h.Id,
            Name = h.Type
        }).ToListAsync();

        var usersFollowed = user.Following.Select(p => p.FollowedId).ToList();
        var pictures = await _data.Pictures.Where(p => usersFollowed.Contains(p.Owner.Id)).Select(p => new PictureVisualizeViewModel()
        {
            Id = p.Id.ToString(),
            PictureUrl = Path.GetFileName(p.Url),
        }).ToListAsync();

        pictures = FilterBrokenUrls.FilterAsync(pictures);

        ExploreViewModel model = new ExploreViewModel()
        {
            HashTags = hashtags,
            PicturesIds = pictures.Skip(skip).Take(pageSize).ToList()
        };
        return model;
      
    }

    public Task<ExploreViewModel> SortByTagAsync(int[] tagsIds, int page, string username)
    {
        throw new NotImplementedException();
    }
}

