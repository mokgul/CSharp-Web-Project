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


    public async Task<ExploreViewModel> GetExploreViewModelAsync(string sort, int page = 1)
    {
        int pageSize = 20;
        int skip = (page - 1) * pageSize;
        var hashtags = await _data.HashTags.Include(h => h.PicturesHashTags).OrderByDescending(h => h.PicturesHashTags.Count).Select(h => new HashTagViewModel()
        {
            Id = h.Id,
            Name = h.Type.Replace("_", " "),
            PicturesCount = h.PicturesHashTags.Count
        }).Take(14).ToListAsync();
        var dropDownMenuTags = await _data.HashTags.Select(h => new HashTagViewModel()
        {
            Id = h.Id,
            Name = h.Type.Replace("_", " ")
        }).ToListAsync();

        var pictures = await _data.Pictures
        .Include(t => t.PicturesHashTags)
        .OrderByDescending(p => p.CreatedOn)
        .Skip(skip)
        .Take(pageSize)
        .Select(p => new PictureVisualizeViewModel()
        {
            Id = p.Id.ToString(),
            Owner = p.Owner.UserName,
            PictureUrl = Path.GetFileName(p.Url),
            CreatedOn = p.CreatedOn,
            Likes = p.Likes,
            HashTags = p.PicturesHashTags.Select(h => h.Tag.Type).ToList()
        })
    .ToListAsync();


        pictures = FilterBrokenUrls.FilterAsync(pictures);

        pictures = await SortPicturesAsync(sort, pictures);

        ExploreViewModel model = new ExploreViewModel()
        {
            HashTags = hashtags,
            TagsForDropDown = dropDownMenuTags,
            Pictures = pictures
        };


        return model;
    }

    private async Task<List<PictureVisualizeViewModel>> SortPicturesAsync(string sort, List<PictureVisualizeViewModel> pictures)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return pictures;
        }
        var owner = string.Empty;
        var tag = string.Empty;
        if (sort != "likes" && sort != "newest" && sort != "oldest")
        {
            if (!_data.HashTags.Any(h => h.Type == sort.Replace(" ", "_")))
            {
                owner = sort;
                sort = "author";
            }
            else
            {
                tag = sort.Replace(" ", "_");
                sort = "tag";
            }
        }
        if (sort != "likes" && sort != "newest" && sort != "oldest" && sort != "tag")
        {
            if (!_data.Users.Any(u => u.UserName == owner))
                throw new ArgumentException($"User {owner} does not exist.");
        }

        switch (sort)
        {
            case "likes":
                pictures = pictures.OrderByDescending(p => p.Likes).ToList();
                break;
            case "newest":
                pictures = pictures.OrderByDescending(p => p.CreatedOn).ToList();
                break;
            case "oldest":
                pictures = pictures.OrderBy(p => p.CreatedOn).ToList();
                break;
            case "author":
                pictures = pictures.Where(p => p.Owner == owner).ToList();
                break;
            case "tag":
                tag = tag.Replace(" ", "_");
                pictures = pictures.Where(p => p.HashTags.Contains(tag)).ToList();
                break;
            default:
                break;
        }
        return pictures;
    }

}

