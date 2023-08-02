namespace ArtfulAdventures.Web.Areas.Admin.Services.Interfaces;

public interface IManageContentService
{
    Task<string> DeletePictureAsync(string pictureId, string user);

    Task DeleteBlogAsync(string blogId, string user);

    Task DeleteCommentPictureAsync(string pictureId, string commentId);

    Task DeleteCommentBlogAsync(string blogId, string commentId);

}

