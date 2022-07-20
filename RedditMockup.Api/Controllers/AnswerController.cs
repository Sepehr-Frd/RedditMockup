using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedditMockup.Api.Base;
using RedditMockup.Api.Filters;
using RedditMockup.Business.Businesses;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Api.Controllers;

public class AnswerController : BaseController<Answer, AnswerDto>
{
    private readonly AnswerBusiness _answerBusiness;

    public AnswerController(IBaseBusiness<Answer, AnswerDto> business) : base(business) =>
        _answerBusiness = (AnswerBusiness)business;

    [Authorization]
    [HttpPost]
    [Route("SubmitVote")]
    public async Task<SamanSalamatResponse?> SubmitVoteAsync([FromQuery] int answerId, bool kind, CancellationToken cancellationToken) =>
    await _answerBusiness.SubmitVoteAsync(answerId, kind, cancellationToken);

    [HttpGet]
    [Route("Votes")]
    [AllowAnonymous]
    public async Task<SamanSalamatResponse?> GetVotesAsync([FromQuery] int answerId, CancellationToken cancellationToken) =>
        await _answerBusiness.LoadVotesAsync(answerId, cancellationToken);
}