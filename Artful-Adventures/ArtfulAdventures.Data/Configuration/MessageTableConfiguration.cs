namespace ArtfulAdventures.Data.Configuration;

using ArtfulAdventures.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MessageTableConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder
         .HasOne(m => m.Sender)
         .WithMany(u => u.SentMessages)
         .HasForeignKey(m => m.SenderId);

        builder
            .HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverId);
    }
}
