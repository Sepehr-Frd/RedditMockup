﻿using Microsoft.EntityFrameworkCore;
using RedditMockup.DataAccess.Base;
using RedditMockup.DataAccess.Context;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess.Repositories;

public class RoleRepository : BaseRepository<Role>
{
    #region [Constructor]

    public RoleRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) : base(context, sieveProcessor)
    {
    }

    #endregion
}