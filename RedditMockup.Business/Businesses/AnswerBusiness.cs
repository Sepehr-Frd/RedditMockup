using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using RedditMockup.Business.Base;
using RedditMockup.Common.Dtos;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;

namespace RedditMockup.Business.Businesses;

public class AnswerBusiness : BaseBusiness<Answer, AnswerDto>
{
    private readonly AnswerRepository _answerRepository;
    private readonly QuestionRepository _questionRepository;
    private readonly AnswerVoteRepository _answerVoteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AnswerBusiness(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, unitOfWork.AnswerRepository!, mapper)
    {
        _answerRepository = unitOfWork.AnswerRepository!;
        _questionRepository = unitOfWork.QuestionRepository!;
        _answerVoteRepository = unitOfWork.AnswerVoteRepository!;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SamanSalamatResponse?> SubmitAnswerAsync(int questionId, AnswerDto answerDto, HttpContext httpContext, CancellationToken cancellationToken = new())
    {
        var question = await _questionRepository.GetByIdAsync(questionId, cancellationToken);

        if (question == null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No question found with id of {questionId}"
            };
        }

        var answer = _mapper.Map<Answer>(answerDto);

        answer.QuestionId = questionId;

        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        answer.UserId = int.Parse(userId);

        var createdAnswer = await _answerRepository.CreateAsync(answer, cancellationToken);

        answer.AnsweringUser!.Score += 1;

        await _unitOfWork.CommitAsync(cancellationToken);

        question.Answers.Add(createdAnswer);

        return new SamanSalamatResponse()
        {
            Data = _mapper.Map<AnswerDto>(createdAnswer),
            IsSuccess = true,
            Message = "Answer successfully submitted"
        };
    }

    public async Task<SamanSalamatResponse?> SubmitVoteAsync(int answerId, bool kind, CancellationToken cancellationToken = new())
    {
        var answer = await _answerRepository.GetByIdAsync(answerId, cancellationToken);

        if (answer == null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No answer found with id of {answerId}"
            };
        }

        if (kind)
        {
            answer.AnsweringUser!.Score += 1;
        }

        var vote = new AnswerVote()
        {
            Kind = kind,
            AnswerId = answer.Id
        };

        var createdVote = await _answerVoteRepository.CreateAsync(vote, cancellationToken);

        answer.Votes.Add(createdVote);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new SamanSalamatResponse()
        {
            IsSuccess = true,
            Message = "Vote submitted"
        };


    }

}