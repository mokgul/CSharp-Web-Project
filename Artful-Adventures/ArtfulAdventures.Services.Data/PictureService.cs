namespace ArtfulAdventures.Services.Data;

using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

using ArtfulAdventures.Data;
using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Services.Data.Interfaces;
using ArtfulAdventures.Web.ViewModels.Comment;
using ArtfulAdventures.Web.ViewModels.HashTag;
using ArtfulAdventures.Web.ViewModels.Picture;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class PictureService : IPictureService
{
    private readonly ArtfulAdventuresDbContext _data;

    public PictureService(ArtfulAdventuresDbContext data)
    {
        _data = data;
    }

    public async Task<PictureAddFormModel> GetPictureAddFormModelAsync()
    {
        var hashtags = await _data.HashTags.Select(h => new HashTagViewModel()
        {
            Id = h.Id,
            Name = h.Type
        }).ToListAsync();

        PictureAddFormModel model = new PictureAddFormModel()
        {
            HashTags = hashtags
        };
        return model;
    }

    public async Task UploadPictureAsync(PictureAddFormModel model, string userId, string path)
    {
        var seletedHashTags = GetSelectedHashtags(model);
        ApplicationUser? user = await GetUser(userId);
        Picture picture = new Picture()
        {
            Url = path,
            UserId = user.Id,
            Owner = user,
            CreatedOn = DateTime.UtcNow,
            Likes = 0,
            Description = model.Description,
        };
        picture.Portfolio.Add(new ApplicationUserPicture()
        {
            UserId = user.Id,
            User = user,
            PictureId = picture.Id,
            Picture = picture
        });

        foreach (var tag in seletedHashTags)
        {
            picture.PicturesHashTags.Add(new PictureHashTag()
            {
                Picture = picture,
                PictureId = picture.Id,
                Tag = await _data.HashTags.FirstOrDefaultAsync(h => h.Id == tag.Id),
                TagId = tag.Id
            });
        }

        await _data.Pictures.AddAsync(picture);
        await _data.SaveChangesAsync();
    }

    private async Task<ApplicationUser?> GetUser(string userId)
    {
        return await _data.Users.Include(m => m.Portfolio).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
    }

    private List<HashTagViewModel> GetSelectedHashtags(PictureAddFormModel model)
    {
        var seletedHashTags = model.HashTags.Where(h => h.IsSelected).ToList();
        return seletedHashTags;
    }

    public async Task<PictureDetailsViewModel> GetPictureDetailsAsync(string id)
    {
        //Get picture with comments and hashtags
        var picture = await _data.Pictures
            .Include(p => p.PicturesHashTags)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id.ToString() == id);

        //Get picture owner with followers and following
        //Since the picture object has a foreign key relationship with the User table (through the UserId property), Entity Framework Core will automatically populate the Owner property of the picture
        var owner = await _data.Users
            .Include(p => p.Followers)
            .Include(p => p.Following)
            .Include(p => p.Portfolio)
            .FirstOrDefaultAsync(u => u.Id == picture.UserId);

        var hashtags = picture.PicturesHashTags.Select(h => new HashTagViewModel()
        {
            Id = h.TagId,
            Name = _data.HashTags.FirstOrDefault(t => t.Id == h.TagId ).Type,
        }).ToList();

        var comments = _data.Comments.Where(c => c.PictureId.ToString() == id).Select(c => new CommentViewModel()
        {
            Id = c.Id,
            Content = c.Content,
            CreatedOn = c.CreatedOn,
            Author = c.Author,
        }).ToList();
        foreach (var comment in comments)
        {
            comment.AuthorPictureUrl = Path.GetFileName(_data.Users.FirstOrDefault(u => u.UserName == comment.Author).Url);
        }

        var model = new PictureDetailsViewModel()
        {
            Id = picture.Id.ToString(),
            Url = Path.GetFileName(picture.Url),
            Owner = picture.Owner.UserName,
            OwnerPictureUrl = Path.GetFileName(picture.Owner.Url),
            OwnerLevel = "Artist",
            OwnerPicturesCount = picture.Owner.Portfolio.Count,
            OwnerFollowersCount = picture.Owner.Followers.Count,
            OwnerFollowingCount = picture.Owner.Following.Count,
            Likes = picture.Likes,
            Description = picture.Description,
            HashTags = hashtags,
            CreatedOn = picture.CreatedOn,
            CommentsCount = comments.Count,
            Comments = comments
        };
        return model;
    }

    public async Task<string> AddToCollectionAsync(string id, string userId)
    {
        var picture = _data.Pictures.FirstOrDefault(p => p.Id.ToString() == id);
        var user = _data.Users.Include(p => p.Collection).FirstOrDefault(u => u.Id.ToString() == userId);
        if(user!.Collection.Any(c => c.PictureId == picture!.Id))
        {
            return string.Empty;
        }
        user!.Collection.Add(new ApplicationUserCollection()
        {
            UserId = user.Id,
            User = user,
            PictureId = picture!.Id,
            Picture = picture
        });
        await  _data.SaveChangesAsync();
        return "You added this picture to your collection.";
    }

    public async Task LikePictureAsync(string pictureId)
    {
        var picture = _data.Pictures.FirstOrDefault(p => p.Id.ToString() == pictureId);
        picture!.Likes++;
        await _data.SaveChangesAsync();
    }
}

