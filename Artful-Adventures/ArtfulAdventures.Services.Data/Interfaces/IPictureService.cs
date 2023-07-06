﻿namespace ArtfulAdventures.Services.Data.Interfaces;

using ArtfulAdventures.Web.ViewModels.Picture;

public interface IPictureService
{
    public Task<PictureAddFormModel> GetPictureAddFormModelAsync();
    Task UploadPictureAsync(PictureAddFormModel model, string id, string path);

}
