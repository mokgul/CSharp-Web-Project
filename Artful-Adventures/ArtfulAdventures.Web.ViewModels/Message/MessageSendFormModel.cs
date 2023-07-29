namespace ArtfulAdventures.Web.ViewModels.Message;

using System.ComponentModel.DataAnnotations;

using static ArtfulAdventures.Common.DataModelsValidationConstants.MessageConstants;

public class MessageSendFormModel
{
    [Required]
    [StringLength(SubjectMaxLength, MinimumLength = SubjectMinLength)]
    public string Subject { get; set; } = null!;

    [Required]
    [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
    public string Content { get; set; } = null!;

    public string Receiver { get; set; } = null!;

}

