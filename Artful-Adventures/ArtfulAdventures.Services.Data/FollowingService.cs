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
        pictures = pictures.OrderByDescending(p => p.CreatedOn).ToList();

        ExploreViewModel model = new ExploreViewModel()
        {
            HashTags = hashtags,
            PicturesIds = pictures.Skip(skip).Take(pageSize).ToList()
        };
        return model;
      
    }

    public async Task<ExploreViewModel> SortByTagAsync(int[] tagsIds, int page, string username)
    {
        var selectedHashTagsIds = tagsIds;

        int pageSize = 20;
        int skip = (page - 1) * pageSize;

        var user = await _data.Users.Include(f => f.Following).FirstOrDefaultAsync(u => u.UserName == username);

        var usersFollowed = user.Following.Select(p => p.FollowedId).ToList();
        var pictureIds = await _data.PicturesHashTags.Where(p => selectedHashTagsIds.Contains(p.TagId)).Select(p => p.PictureId).ToListAsync();
        var pictures = await _data.Pictures.Where(p => usersFollowed.Contains(p.Owner.Id)).Where(p => pictureIds.Contains(p.Id)).Select(p => new PictureVisualizeViewModel()
        {
            Id = p.Id.ToString(),
            PictureUrl = Path.GetFileName(p.Url),
        }).ToListAsync();
        ExploreViewModel model = new ExploreViewModel();
        pictures = FilterBrokenUrls.FilterAsync(pictures);
        model.HashTags = await _data.HashTags.Select(h => new HashTagViewModel()
        {
            Id = h.Id,
            Name = h.Type
        }).ToListAsync();
        model.PicturesIds = pictures.Skip(skip).Take(pageSize).ToList();

        return model;
    }
}

