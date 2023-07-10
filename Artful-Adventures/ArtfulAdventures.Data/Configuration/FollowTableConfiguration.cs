namespace ArtfulAdventures.Data.Configuration;

using ArtfulAdventures.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FollowTableConfiguration : IEntityTypeConfiguration<FollowerFollowing>
{
    public void Configure(EntityTypeBuilder<FollowerFollowing> builder)
    {
        builder
            .HasKey(f => new { f.FollowerId, f.FollowedId });

        builder
            .HasOne(f => f.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(f => f.Followed)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

