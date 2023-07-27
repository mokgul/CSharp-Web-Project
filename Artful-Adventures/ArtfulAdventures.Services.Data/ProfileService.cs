namespace ArtfulAdventures.Services.Data;

using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;

using ArtfulAdventures.Data;
using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Services.Data.Interfaces;
using ArtfulAdventures.Web.ViewModels.Picture;
using ArtfulAdventures.Web.ViewModels.Skill;
using ArtfulAdventures.Web.ViewModels.UserProfile;

using Microsoft.EntityFrameworkCore;

public class ProfileService : IProfileService
{
    private readonly ArtfulAdventuresDbContext _data;

    public ProfileService(ArtfulAdventuresDbContext data)
    {
        _data = data;
    }

    public async Task<string> FollowAsync(string username, string userId)
    {
        var userVisited = await _data.Users.Include(m => m.Followers).Include(s => s.Following).FirstOrDefaultAsync(u => u.UserName == username);

        var userVisitor = await _data.Users.Include(m => m.Followers).Include(s => s.Following).FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        if (userVisited!.Id == userVisitor!.Id)
        {
            return string.Empty;
        }
        var userFollow = new FollowerFollowing()
        {
            Follower = userVisitor,
            FollowerId = userVisitor!.Id,
            Followed = userVisited,
            FollowedId = userVisited!.Id,
        };

        if (!userVisited!.Followers.Any(f => f.FollowerId == userVisitor!.Id)
            && !userVisitor!.Following.Any(f => f.FollowedId == userVisited!.Id))
        {
            userVisited!.Followers.Add(userFollow);
            userVisitor!.Following.Add(userFollow!);
            await _data.SaveChangesAsync();
        };
        return "Success";
    }

    public async Task<PortfolioViewModel?> GetCollectionAsync(string username)
    {
        // Query the Users DbSet
        var user = await _data.Users
            // Include the Collection navigation property
            .Include(p => p.Collection)
            // Then include the Picture navigation property of the Collection
            .ThenInclude(p => p.Picture)
            // Find the first user with the specified username
            .FirstOrDefaultAsync(u => u.UserName == username);


        if (user!.Collection.Count == 0)
        {
            return null;
        }
        var pictures = user!.Collection.Select(p => new PictureVisualizeViewModel()
        {
            
            Id = p.PictureId.ToString(),
            CreatedOn = p.Picture.CreatedOn,
            Likes = p.Picture.Likes,
            PictureUrl = Path.GetFileName(p.Picture!.Url),
        }).ToList();

        pictures = FilterBrokenUrls.FilterAsync(pictures);
        pictures = pictures.OrderByDescending(p => p.CreatedOn).ToList();
        var model = new PortfolioViewModel()
        {
            Pictures = pictures,
        };
        return model;
    }

    public async Task<FollowViewModel?> GetFollowersAsync(string username)
    {
        var user = await _data.Users.Include(m => m.Followers).Include(m => m.Following).FirstOrDefaultAsync(u => u.UserName == username);

        if (user!.Followers.Count == 0)
        {
            return null;
        }
        var followers = user!.Followers.Select(f => new ProfilePartialView()
        {
            Username = _data.Users.FirstOrDefault(u => u.Id == f.FollowerId)!.UserName!,
            ProfilePictureUrl = Path.GetFileName(f.Follower.Url),
            Name = f.Follower.Name,
            Bio = f.Follower.Bio,
            CityName = f.Follower.CityName,
            FollowersCount = _data.Users.Include(p => p.Followers).FirstOrDefault(u => u.Id == f.FollowerId)!.Followers.Count(),
            FollowingCount = _data.Users.Include(p => p.Following).FirstOrDefault(u => u.Id == f.FollowerId)!.Following.Count(),
            PicturesCount = _data.Users.Include(p => p.Portfolio).FirstOrDefault(u => u.Id == f.FollowerId)!.Portfolio.Count(),
        }).ToList();

        var model = new FollowViewModel()
        {
            Followers = followers,
        };
        return model;
    }

    public async Task<FollowViewModel?> GetFollowingAsync(string username)
    {
        var user = await _data.Users.Include(m => m.Followers).Include(m => m.Following).FirstOrDefaultAsync(u => u.UserName == username);

        if (user!.Following.Count == 0)
        {
            return null;
        }
        var following = user!.Following.Select(f => new ProfilePartialView()
        {
            Username = _data.Users.FirstOrDefault(u => u.Id == f.FollowedId)?.UserName!,
            ProfilePictureUrl = Path.GetFileName(f.Followed.Url),
            Name = f.Followed.Name,
            Bio = f.Followed.Bio,
            CityName = f.Followed.CityName,
            FollowersCount = _data.Users.Include(p => p.Followers).FirstOrDefault(u => u.Id == f.FollowedId)!.Followers.Count(),
            FollowingCount = _data.Users.Include(p => p.Following).FirstOrDefault(u => u.Id == f.FollowedId)!.Following.Count(),
            PicturesCount = _data.Users.Include(p => p.Portfolio).FirstOrDefault(u => u.Id == f.FollowedId)!.Portfolio.Count(),
        }).ToList();

        var model = new FollowViewModel()
        {
            Followers = following,
        };
        return model;
    }

    public async Task<PortfolioViewModel?> GetPortfolioAsync(string username)
    {
        var user = await _data.Users
                .Include(p => p.Portfolio)
                .ThenInclude(p => p.Picture)
                .FirstOrDefaultAsync(u => u.UserName == username);

        if (user!.Portfolio.Count == 0)
        {
            return null;
        }

        var pictures = user!.Portfolio.Select(p => new PictureVisualizeViewModel()
        {
            Id = p.PictureId.ToString(),
            CreatedOn = p.Picture.CreatedOn,
            Likes = p.Picture.Likes,
            PictureUrl = Path.GetFileName(p.Picture!.Url),
        }).ToList();

        pictures = FilterBrokenUrls.FilterAsync(pictures);
        pictures = pictures.OrderByDescending(p => p.CreatedOn).ToList();

        var model = new PortfolioViewModel()
        {
            Pictures = pictures,
        };
        return model;
    }

    public async Task<ProfileViewModel?> GetProfileViewModelAsync(string username, string userId)
    {
        var user = await _data.Users
                .Include(m => m.Followers)
                .Include(s => s.Following)
                .Include(p => p.Portfolio)
                .ThenInclude(p => p.Picture)
                .Include(s => s.ApplicationUsersSkills)
                .ThenInclude(s => s.Skill)
                .FirstOrDefaultAsync(u => u.UserName == username);
        if (user == null)
        {
            return null;
        };

        var visitor = await _data.Users.Include(m => m.Followers).Include(s => s.Following).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        var followed = false;
        if (user.Id != visitor!.Id)
        {
            followed = visitor.Following.Any(f => f.FollowedId == user.Id);
        }
        ICollection<SkillViewModel> skills = user!.ApplicationUsersSkills.Select(sa => new SkillViewModel()
        {
            Id = sa.SkillId,
            Name = sa.Skill.Type,
        }).ToList();

        var pictures = user!.Portfolio.Select(p => new PictureVisualizeViewModel()
        {
            Id = p.PictureId.ToString(),
            CreatedOn = p.Picture.CreatedOn,
            Likes = p.Picture.Likes,
            PictureUrl = Path.GetFileName(p.Picture!.Url),
        }).ToList();

        pictures = FilterBrokenUrls.FilterAsync(pictures);
        pictures = pictures.OrderByDescending(p => p.CreatedOn).ToList();

        var model = new ProfileViewModel()
        {
            Username = user.UserName,
            Email = user.Email,
            ProfilePictureUrl = Path.GetFileName(user.Url),
            Name = user.Name,
            Bio = user.Bio,
            About = user.About,
            CityName = user.CityName,
            Skills = skills,
            Pictures = pictures,
            isFollowed = followed,
            FollowersCount = user.Followers.Count,
            FollowingCount = user.Following.Count,
            PicturesCount = user.Portfolio.Count,
        };
        return model;
    }

    public async Task UnfollowAsync(string username, string userId)
    {
        var userVisited = await _data.Users.Include(m => m.Followers).Include(s => s.Following).FirstOrDefaultAsync(u => u.UserName == username);

        var userVisitor = await _data.Users.Include(m => m.Followers).Include(s => s.Following).FirstOrDefaultAsync(u => u.Id.ToString() == userId);


        if (userVisited!.Followers.Any(f => f.FollowerId == userVisitor!.Id)
            && userVisitor!.Following.Any(f => f.FollowedId == userVisited!.Id))
        {
            var userFollow = userVisited!.Followers.FirstOrDefault(f => f.FollowerId == userVisitor!.Id);
            userVisited!.Followers.Remove(userFollow!);
            userVisitor!.Following.Remove(userFollow!);
            await _data.SaveChangesAsync();
        };

    }
}

