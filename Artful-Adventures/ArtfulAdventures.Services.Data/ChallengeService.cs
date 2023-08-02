namespace ArtfulAdventures.Services.Data;

using System.Threading.Tasks;

using ArtfulAdventures.Data;
using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Services.Data.Interfaces;
using ArtfulAdventures.Web.ViewModels.Challenges;
using static ArtfulAdventures.Common.GeneralApplicationConstants;

using Microsoft.EntityFrameworkCore;

public class ChallengeService : IChallengeService
{
    private readonly ArtfulAdventuresDbContext _data;

    public ChallengeService(ArtfulAdventuresDbContext data)
    {
        _data = data;
    }

    public async Task<ChallengesViewModel> GetAllAsync(int page = 1)
    {
        int pageSize = 6;
        int skip = (page - 1) * pageSize;
        var challenges = await _data.Challenges.Select(c => new ChallengeVisualizeViewModel()
        {
            Id = c.Id,
            Title = c.Title,
            Creator = c.Creator,
            CreatedOn = c.CreatedOn,
            Participants = c.Participants,
            PictureUrl = Path.GetFileName(c.Url),
        })
        .Skip(skip).Take(pageSize).ToListAsync();

        challenges = challenges.OrderByDescending(x => x.CreatedOn).ToList();

        var model = new ChallengesViewModel()
        {
            Challenges = challenges
        };
        return model;
    }

    public async Task<ChallengeDetailsViewModel> GetChallengeDetailsAsync(int id)
    {
        var challenge = await _data.Challenges.FindAsync(id);
        var pictures = await _data.Pictures
            .Where(x => x.ChallengeId == id)
            .ToDictionaryAsync(x => x.Id.ToString(), x => Path.GetFileName(x.Url));
        if(challenge == null)
        {
            throw new NullReferenceException();
        }

        var model = new ChallengeDetailsViewModel
        {
            Id = challenge.Id,
            Title = challenge.Title,
            Requirements = challenge.Requirements,
            Url = Path.GetFileName(challenge.Url),
            CreatedOn = challenge.CreatedOn,
            Creator = challenge.Creator,
            Participants = challenge.Participants,
            Pictures = pictures
        };
        return model;
    }

    public async Task ParticipateAsync(int id, string userId, string path)
    {
        var challenge = await _data.Challenges.FindAsync(id);
        if(challenge == null)
        {
            throw new NullReferenceException("Challenge not found");
        }

        var user = await _data.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);  
        if(user == null)
        {
            throw new NullReferenceException("User not found");
        }

        var picture = new Picture
        {
            Url = path,
            Owner = user!,
            UserId = user!.Id,
            CreatedOn = DateTime.UtcNow,
            Likes = 0,
            Description = defaultPictureDescriptionChallenge,
            Challenge = challenge,
            ChallengeId = challenge.Id,
        };
        picture.Portfolio.Add(new ApplicationUserPicture()
        {
            UserId = user.Id,
            User = user,
            PictureId = picture.Id,
            Picture = picture
        });

        challenge.Participants++;
        challenge.Pictures.Add(picture);
        await _data.Pictures.AddAsync(picture);
        await _data.SaveChangesAsync();

    }
}

