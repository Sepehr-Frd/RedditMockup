﻿using RedditMockup.Model.BaseEntities;

namespace RedditMockup.Model.Entities;

public class Bookmark : BaseEntity
{
    // [Properties]
    public bool IsBookmarked { get; set; }

    // --------------------------------------

    // [Navigation Properties]

    public int UserId { get; set; }

    public User? User { get; set; }

    public int QuestionId { get; set; }

    public Question? Question { get; set; }

    // --------------------------------------
}