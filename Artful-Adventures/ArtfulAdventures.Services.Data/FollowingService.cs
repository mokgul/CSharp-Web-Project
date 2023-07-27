﻿namespace ArtfulAdventures.Services.Data;

using System.Linq;
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
    public async Task<ExploreViewModel> GetExploreViewModelAsync(string sort, int page, string username)
    {
        int pageSize = 20;
        int skip = (page - 1) * pageSize;
        var user = await _data.Users.Include(f => f.Following).FirstOrDefaultAsync(u => u.UserName == username);

        var dropDownMenuTags = await _data.HashTags.Select(h => new HashTagViewModel()
        {
            Id = h.Id,
            Name = h.Type.Replace("_", " ")
        }).ToListAsync();

        var usersFollowed = user.Following.Select(p => p.FollowedId).ToList();
        var pictures = await _data.Pictures.Include(h => h.PicturesHashTags).Where(p => usersFollowed.Contains(p.Owner.Id)).Select(p => new PictureVisualizeViewModel()
        {
            Id = p.Id.ToString(),
            PictureUrl = Path.GetFileName(p.Url),
            Owner = p.Owner.UserName,
            CreatedOn = p.CreatedOn,
            Likes = p.Likes,
            HashTags = p.PicturesHashTags.Select(h => h.Tag.Type).ToList()
        }).ToListAsync();

        var pictureIds = pictures.Select(p => p.Id).ToList();

        var hashtags = await _data.HashTags.Include(h => h.PicturesHashTags)
    .Where(p => p.PicturesHashTags.Any(p => pictureIds.Contains(p.PictureId.ToString())))
    .OrderByDescending(h => h.PicturesHashTags.Count)
    .Select(h => new HashTagViewModel()
    {
        Id = h.Id,
        Name = h.Type.Replace("_", " "),
        PicturesCount = h.PicturesHashTags.Count(p => pictureIds.Contains(p.PictureId.ToString()))
    }).Take(14).ToListAsync();


        pictures = FilterBrokenUrls.FilterAsync(pictures);
        pictures = pictures.OrderByDescending(p => p.CreatedOn).ToList();

        pictures = await SortPicturesAsync(sort, pictures);

        ExploreViewModel model = new ExploreViewModel()
        {
            HashTags = hashtags,
            TagsForDropDown = dropDownMenuTags,
            Pictures = pictures.Skip(skip).Take(pageSize).ToList()
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

            if (!pictures.Any(p => p.Owner == owner) && pictures.Count(p => p.Owner == owner) > 0)
                throw new ArgumentException($"You are not following user {owner}.");

            if (pictures.Count(p => p.Owner == owner) == 0 )
                throw new ArgumentException($"{owner} has not uploaded any pictures yet.");

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

