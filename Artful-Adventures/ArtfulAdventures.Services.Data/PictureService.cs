namespace ArtfulAdventures.Services.Data;

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
        var picture = await _data.Pictures
            .Include(p => p.PicturesHashTags)
            .Include(p => p.Portfolio)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id.ToString() == id);

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
            Author = c.Author
        }).ToList();

        var model = new PictureDetailsViewModel()
        {
            Id = picture.Id.ToString(),
            Url = Path.GetFileName(picture.Url),
            Owner = picture.Owner.UserName,
            OwnerPictureUrl = Path.GetFileName(picture.Owner.Url),
            Likes = picture.Likes,
            Description = picture.Description,
            HashTags = hashtags,
            CreatedOn = picture.CreatedOn,
            Comments = comments
        };
        return model;
    }
}

