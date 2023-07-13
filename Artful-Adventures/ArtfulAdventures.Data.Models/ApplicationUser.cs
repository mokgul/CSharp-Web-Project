namespace ArtfulAdventures.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

using static ArtfulAdventures.Common.DataModelsValidationConstants.ApplicationUserConstants;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        this.Id = Guid.NewGuid();
        ApplicationUsersSkills = new HashSet<ApplicationUserSkill>();
        Portfolio = new HashSet<ApplicationUserPicture>();
    }

    [MaxLength(UrlMaxLength)]
    public string? Url { get; set; }

    [MaxLength(NameMaxLength)]
    public string? Name { get; set; }


    [MaxLength(BioMaxLength)]
    public string? Bio { get; set; }

    [MaxLength(CityNameMaxLength)]
    public string? CityName { get; set; }

    [MaxLength(AboutMaxLength)]
    public string? About { get; set; }

    //public ICollection<Picture> Portfolio { get; set; } = new HashSet<Picture>();

    public ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();

    public ICollection<FollowerFollowing> Followers { get; set; } = new HashSet<FollowerFollowing>();

    public ICollection<FollowerFollowing> Following { get; set; } = new HashSet<FollowerFollowing>();

    public ICollection<ApplicationUserSkill> ApplicationUsersSkills { get; set; }

    public ICollection<ApplicationUserPicture> Portfolio { get; set; }

    public ICollection<ApplicationUserCollection> Collection { get; set; }


    //Collection And Skills are stored in Mapping Tables
}

