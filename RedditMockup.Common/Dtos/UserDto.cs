﻿namespace RedditMockup.Common.Dtos;

public class UserDto : BaseDto
{
    public string? Username { get; init; }

    public string? Password { get; set; }

    public int Score { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public ProfileDto? Profile { get; init; }
}