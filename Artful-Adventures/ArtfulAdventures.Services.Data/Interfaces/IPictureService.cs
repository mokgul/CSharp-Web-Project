﻿namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Data.Models;
using ArtfulAdventures.Web.ViewModels.Picture;

public interface IPictureService
{
    Task<PictureAddFormModel> GetPictureAddFormModelAsync();

    Task UploadPictureAsync(PictureAddFormModel model, string id, string path);

    Task <PictureDetailsViewModel> GetPictureDetailsAsync(string id);

    Task<string> AddToCollectionAsync(string id, string userId);

    Task<ICollection<PictureEditViewModel>> ManageGetAllPicturesAsync(string userId, int page);

    Task<PictureEditViewModel> GetPictureToEditAsync(string id);

    Task EditPictureAsync(PictureEditViewModel model);

    Task<string> DeletePictureAsync(string id, string userId);

    Task LikePictureAsync(string pictureId);

}

