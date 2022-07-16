using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NLog;
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
    private readonly UserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Logger _logger;

    public AnswerBusiness(IUnitOfWork unitOfWork, IMapper mapper, Logger logger) : base(unitOfWork, unitOfWork.AnswerRepository!, mapper)
    {
        _answerRepository = unitOfWork.AnswerRepository!;
        _questionRepository = unitOfWork.QuestionRepository!;
        _answerVoteRepository = unitOfWork.AnswerVoteRepository!;
        _userRepository = unitOfWork.UserRepository!;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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

        var stringUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        int userId = int.Parse(stringUserId);

        var answeringUser = await _userRepository.GetByIdAsync(userId);

        var createdAnswer = await _answerRepository.CreateAsync(answer, cancellationToken);

        if (answeringUser is null)
        {
            _logger.Error("AnswerRepository returned null for CreateAsync inside AnswerBusiness");
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "Internal Server Error - 500"
            };
        }

        answeringUser!.Answers.Add(createdAnswer);

        answeringUser!.Score += 1;

        question.Answers.Add(createdAnswer);

        await _unitOfWork.CommitAsync(cancellationToken);

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

    public async Task<SamanSalamatResponse?> UpdateAsync(int answerId, int questionId, AnswerDto answerDto, CancellationToken cancellationToken = new())
    {
        var question = await _questionRepository.GetByIdAsync(questionId, cancellationToken);

        if (question is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "No question found with given question ID"
            };
        }

        var answer = question.Answers.SingleOrDefault(answer => answer.Id == answerId);

        if (answer is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "No answer found with given answer ID"
            };
        }

        answer.Title = answerDto.Title;

        answer.Description = answerDto.Description;

        var response = await _answerRepository.UpdateAsync(answer, cancellationToken);

        return new SamanSalamatResponse()
        {
            Data = response,
            IsSuccess = true,
            Message = "Successfully updated the answer. New answer:"
        };

    }
}