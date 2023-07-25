﻿namespace ArtfulAdventures.Web.ViewModels.Blog;

using ArtfulAdventures.Web.ViewModels.Comment;

public class BlogDetailsViewModel
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ImageUrl { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int Likes { get; set; }

    public int CommentsCount { get; set; }

    public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
}
