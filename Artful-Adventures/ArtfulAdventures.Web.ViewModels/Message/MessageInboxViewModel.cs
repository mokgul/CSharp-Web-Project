namespace ArtfulAdventures.Web.ViewModels.Message;


public class MessageInboxViewModel
{
    public int Id { get; set; }

    public string Subject { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Sender { get; set; } = null!;

    public string Receiver { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public bool IsRead { get; set; }

    public int UnreadMessages { get; set; }

    public ICollection<MessageInboxViewModel> MessagesHistory { get; set; } = new List<MessageInboxViewModel>();
}

