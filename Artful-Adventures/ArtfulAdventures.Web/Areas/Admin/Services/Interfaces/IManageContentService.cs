namespace ArtfulAdventures.Web.Areas.Admin.Services.Interfaces;

using ArtfulAdventures.Web.Areas.Admin.Models;

public interface IManageContentService
{
    Task<string> DeletePictureAsync(string pictureId, string user);

    Task DeleteBlogAsync(string blogId, string user);

    Task DeleteCommentPictureAsync(string pictureId, string commentId);

    Task DeleteCommentBlogAsync(string blogId, string commentId);

    Task<ChallengeCreateFormModel> CreateChallengeGetFormAsync(string userId);

    Task<int> CreateChallengeAsync(ChallengeCreateFormModel model, string path);

    Task<string> DeleteChallengeAsync(int challengeId);

}

