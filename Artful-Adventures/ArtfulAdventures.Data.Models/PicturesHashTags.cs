﻿namespace ArtfulAdventures.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PicturesHashTags
{
    [Required]
    [ForeignKey(nameof(Picture))]
    public Guid PictureId { get; set; }

    public Picture Picture { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Tag))]
    public int TagId { get; set; }

    public HashTag Tag { get; set; } = null!;
}

