﻿namespace Application.Photos;

public class PhotoDetailsEntity
{
    public int Id { get; set; }
    public int PhotoId { get; set; }
    public int Likes { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }

    // TODO: Add Like behavior.
}