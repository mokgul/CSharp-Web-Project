namespace ArtfulAdventures.Web.Controllers;

using ArtfulAdventures.Data;
using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Web.ViewModels.Message;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class MessageController : Controller
{
    private readonly ArtfulAdventuresDbContext _data;

    public MessageController(ArtfulAdventuresDbContext data)
    {
        _data = data;
    }

    [HttpGet]
    public async Task<IActionResult> SendMessage(string username)
    {
        var model = new MessageSendFormModel();
        model.Receiver = username;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(MessageSendFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var receiver = await _data.Users.FirstOrDefaultAsync(x => x.UserName == model.Receiver);
        if (receiver == null)
        {
            ModelState.AddModelError("Receiver", "User does not exist.");
            return View(model);
        }
        var user = await _data.Users.FirstOrDefaultAsync(x => x.UserName == this.User!.Identity!.Name);
        var message = new Message()
        {
            Sender = user!,
            SenderId = user!.Id,
            Receiver = receiver,
            ReceiverId = receiver.Id,
            Subject = model.Subject,
            Content = model.Content,
            Timestamp = DateTime.UtcNow,
        };
        receiver.ReceivedMessages.Add(message);
        await _data.Messages.AddAsync(message);
        await _data.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Inbox()
    {
        var currentUser = User.Identity.Name;
        var messages = await _data.Messages
            .Include(s => s.Sender)
            .Include(r => r.Receiver)
            .Where(m => m.Receiver.UserName == currentUser)
            .ToListAsync();

        var groupedMessages = messages
            .GroupBy(m => m.Sender.UserName)
            .Select(g => new
            {
                Sender = g.Key,
                LatestMessage = g.OrderByDescending(m => m.Timestamp).FirstOrDefault(),
                UnreadCount = g.Count(m => !m.IsRead)
            })
            .ToList();

        var inboxMessages = groupedMessages
            .Select(g => new MessageInboxViewModel
            {
                Id = g.LatestMessage.Id,
                Subject = g.LatestMessage.Subject,
                Content = g.LatestMessage.Content,
                Sender = g.Sender,
                Receiver = g.LatestMessage.Receiver.UserName,
                Timestamp = g.LatestMessage.Timestamp,
                IsRead = g.LatestMessage.IsRead,
                UnreadMessages = g.UnreadCount
            })
            .ToList();

        var model = new MessageInbox
        {
            Messages = inboxMessages,
            TotalUnreadMessages = inboxMessages.Sum(m => m.UnreadMessages)
        };

        return View(model);

    }

    [HttpGet]
    public async Task<IActionResult> ViewMessage(int id)
    {
        var message = await _data.Messages
            .Include(s => s.Sender)
            .Include(r => r.Receiver)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (message == null)
        {
            return RedirectToAction("Index", "Home");
        }
        message.IsRead = true;
        await _data.SaveChangesAsync();
        var model = new MessageInboxViewModel
        {
            Id = message.Id,
            Subject = message.Subject,
            Content = message.Content,
            Sender = message.Sender.UserName,
            Receiver = message.Receiver.UserName,
            Timestamp = message.Timestamp,
            IsRead = message.IsRead
        };
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> ViewMessageHistory(int id)
    {
        var currentUser = User.Identity.Name;
        var message = await _data.Messages.Include(s => s.Sender).Include(r => r.Receiver).Where(s => s.Sender.UserName != currentUser)
            .OrderByDescending(t => t.Timestamp).FirstOrDefaultAsync(m => m.Id == id);
        var secondUser = message.Sender.UserName == currentUser ? message.Receiver.UserName : message.Sender.UserName;
        var messagesHistory = await _data.Messages
            .Where(m => (m.Sender.UserName == currentUser && m.Receiver.UserName == secondUser) ||
                        (m.Sender.UserName == secondUser && m.Receiver.UserName == currentUser))
            .Select(m => new MessageInboxViewModel
            {
                Id = m.Id,
                Subject = m.Subject,
                Content = m.Content,
                Sender = m.Sender.UserName,
                Receiver = m.Receiver.UserName,
                Timestamp = m.Timestamp,
                IsRead = m.IsRead
            })
            .OrderByDescending(m => m.Timestamp)
            .ToListAsync();
        var model = new MessageInboxViewModel()
        {
            Id = message.Id,
            Subject = message.Subject,
            Content = message.Content,
            Sender = message.Sender.UserName,
            Receiver = message.Receiver.UserName,
            Timestamp = message.Timestamp,
            IsRead = message.IsRead,
            MessagesHistory = messagesHistory
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Reply(int id)
    {
        var message = await _data.Messages.Include(s => s.Sender).FirstOrDefaultAsync(x => x.Id == id);
        var currentUser = User.Identity.Name;
        var model = new MessageReplyFormModel()
        {
            Receiver = message.Sender.UserName,
            Subject = message.Subject,
            Content = string.Empty,
        };
        return View(model);
    }



}


