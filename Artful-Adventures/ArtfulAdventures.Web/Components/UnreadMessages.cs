namespace ArtfulAdventures.Web.Components;
using ArtfulAdventures.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class UnreadMessagesViewComponent : ViewComponent
{
    private readonly ArtfulAdventuresDbContext _data;

    public UnreadMessagesViewComponent(ArtfulAdventuresDbContext data)
    {
        _data = data;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var currentUser = User.Identity.Name;
        var totalUnreadMessages = await _data.Messages
            .Include(s => s.Sender)
            .Include(r => r.Receiver)
            .Where(m => m.Receiver.UserName == currentUser && !m.IsRead)
            .CountAsync();
        return View(totalUnreadMessages);
    }
}
