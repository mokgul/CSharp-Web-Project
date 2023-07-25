namespace ArtfulAdventures.Services.Data;

using System.Threading.Tasks;

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


    public async Task<ExploreViewModel> GetExploreViewModelAsync(int page = 1)
    {
        int pageSize = 20;
        int skip = (page - 1) * pageSize;
        var hashtags = await _data.HashTags.Select(h => new HashTagViewModel()
        {
            Id = h.Id,
            Name = h.Type
        }).ToListAsync();

        var pictures = await _data.Pictures.Select(p => new PictureVisualizeViewModel()
        {
            Id = p.Id.ToString(),
            PictureUrl = Path.GetFileName(p.Url),
            CreatedOn = p.CreatedOn,
            Likes = p.Likes,
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

    public async Task<ExploreViewModel> SortByTagAsync(int[] tagsIds, int page = 1)
    {

        var selectedHashTagsIds = tagsIds;

        int pageSize = 20;
        int skip = (page - 1) * pageSize;
        var pictureIds = await _data.PicturesHashTags.Where(p => selectedHashTagsIds.Contains(p.TagId)).Select(p => p.PictureId).ToListAsync();
        var pictures = await _data.Pictures.Where(p => pictureIds.Contains(p.Id)).Select(pv => new PictureVisualizeViewModel()
        {
            Id = pv.Id.ToString(),
            PictureUrl = Path.GetFileName(pv.Url)
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

