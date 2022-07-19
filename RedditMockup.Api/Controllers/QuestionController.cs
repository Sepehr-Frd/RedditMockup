using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedditMockup.Api.Base;
using RedditMockup.Business.Businesses;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Api.Controllers;

public class QuestionController : BaseController<Question, QuestionDto>
{
    private readonly QuestionBusiness _questionBusiness;


    public QuestionController(IBaseBusiness<Question, QuestionDto> business) : base(business)
    {
        _questionBusiness = (QuestionBusiness)business;
    }

    [HttpGet]
    [Route("Answers")]
    [AllowAnonymous]
    public async Task<SamanSalamatResponse?> GetAnswersAsync([FromQuery] int questionId, CancellationToken cancellationToken) =>
        await _questionBusiness.LoadAnswersAsync(questionId, cancellationToken);

    [HttpGet]
    [Route("Votes")]
    [AllowAnonymous]
    public async Task<SamanSalamatResponse?> GetVotesAsync([FromQuery] int questionId, CancellationToken cancellationToken) =>
        await _questionBusiness.LoadVotesAsync(questionId, cancellationToken);

    [HttpPost]
    [Route("SubmitVote")]
    public async Task<SamanSalamatResponse?> SubmitVoteAsync([FromQuery] int questionId, bool kind, CancellationToken cancellationToken) =>
        await _questionBusiness.SubmitVoteAsync(questionId, kind, cancellationToken);
}