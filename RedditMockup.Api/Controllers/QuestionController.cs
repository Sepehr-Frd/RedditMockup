using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using RedditMockup.Api.Base;
using RedditMockup.Business.Businesses;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Api.Controllers;

public class QuestionController : BaseController<Question, QuestionDto>
{
    private readonly QuestionBusiness _questionBusiness;


    public QuestionController(IBaseBusiness<QuestionDto> business) : base(business)
    {
        _questionBusiness = (QuestionBusiness)business;
    }

    [HttpGet]
    [Route("id")]
    [AllowAnonymous]
    public async Task<QuestionDto?> GetQuestionByIdAsync([FromQuery] int id, CancellationToken cancellationToken) =>
        await _questionBusiness.GetByIdAsync(id, cancellationToken);

    [HttpGet]
    [Route("Answers")]
    [AllowAnonymous]
    public async Task<List<AnswerDto>?> GetAnswersAsync([FromQuery] int questionId, CancellationToken cancellationToken) =>
        await _questionBusiness.GetAnswersAsync(questionId, cancellationToken);

    [HttpGet]
    [Route("Votes")]
    [AllowAnonymous]
    public async Task<List<VoteDto>?> GetVotesAsync([FromQuery] int questionId, CancellationToken cancellationToken) =>
        await _questionBusiness.GetVotesAsync(questionId, cancellationToken);

    [HttpPost]
    public async override Task<SamanSalamatResponse?> CreateAsync([FromQuery] QuestionDto dto, CancellationToken cancellationToken) =>
        await _questionBusiness.CreateAsync(dto, HttpContext, cancellationToken);

    [HttpPost]
    [Route("SubmitVote")]
    public async Task<SamanSalamatResponse?> SubmitVoteAsync([FromQuery] int questionId, bool kind, CancellationToken cancellationToken) =>
        await _questionBusiness.SubmitVoteAsync(questionId, kind, cancellationToken);
}