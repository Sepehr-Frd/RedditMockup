﻿using Microsoft.AspNetCore.Mvc;
using RedditMockup.Api.Base;
using RedditMockup.Business.Contracts;
using RedditMockup.Business.DomainEntityBusinesses;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Api.Controllers;

[Route("answers")]
public class AnswerController : BaseController<Answer, AnswerDto>
{
    // [Fields]

    private readonly AnswerBusiness _business;

    // --------------------------------------

    // [Constructor]

    public AnswerController(IBaseBusiness<Answer, AnswerDto> business) : base(business) =>
        _business = (AnswerBusiness)business;

    // --------------------------------------

    // [Methods]

    [HttpGet]
    [Route("{guid}/votes")]
    public async Task<CustomResponse<List<AnswerVote>>> GetVotesByAnswerGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _business.GetVotesByAnswerGuidAsync(guid, cancellationToken);

    [HttpPost]
    [Route("{guid}/votes")]
    public async Task<CustomResponse> SubmitVoteAsync([FromRoute] Guid guid, [FromBody] bool kind, CancellationToken cancellationToken) =>
        await _business.SubmitVoteAsync(guid, kind, cancellationToken);

    // --------------------------------------
}