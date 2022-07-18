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

    public AnswerController(IBaseBusiness<AnswerDto> business) : base(business)
    {
        _answerBusiness = (AnswerBusiness)business;
    }

    [HttpPut]
    public async Task<SamanSalamatResponse?> UpdateAnswerAsync([FromQuery] int answerId, AnswerDto answerDto, CancellationToken cancellationToken) =>
        await _answerBusiness.UpdateAsync(answerDto, cancellationToken);

    [Authorization]
    public async new Task<SamanSalamatResponse?> CreateAsync([FromQuery] AnswerDto dto, CancellationToken cancellationToken) =>
        await _answerBusiness.CreateAsync(dto, HttpContext, cancellationToken);

    [Authorization]
    [HttpPost]
    [Route("SubmitVote")]
    public async Task<SamanSalamatResponse?> SubmitVoteAsync([FromQuery] int answerId, bool kind, CancellationToken cancellationToken) =>
        await _answerBusiness.SubmitVoteAsync(answerId, kind, cancellationToken);
}