namespace ArtfulAdventures.Data.Configuration;

using System.Xml.Linq;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static ArtfulAdventures.Common.DataModelsValidationConstants.RolesConstants;

public class CreateRolesConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder
            .HasData(new IdentityRole<Guid> { Id = Guid.Parse(AdminId), Name = "Administrator", NormalizedName = "ADMINISTRATOR" });

        builder
            .HasData(new IdentityRole<Guid> { Id = Guid.Parse(UserId), Name = "User", NormalizedName = "USER" });
    }

}
