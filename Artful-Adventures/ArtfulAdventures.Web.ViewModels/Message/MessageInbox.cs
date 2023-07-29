namespace ArtfulAdventures.Web.ViewModels.Message;

public class MessageInbox
{
    public ICollection<MessageInboxViewModel> Messages { get; set; } = new List<MessageInboxViewModel>();

    public int TotalUnreadMessages { get; set; }
}

