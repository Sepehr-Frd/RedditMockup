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
    private readonly UserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public AnswerBusiness(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, unitOfWork.AnswerRepository!, mapper)
    {
        _answerRepository = unitOfWork.AnswerRepository!;
        _questionRepository = unitOfWork.QuestionRepository!;
        _answerVoteRepository = unitOfWork.AnswerVoteRepository!;
        _userRepository = unitOfWork.UserRepository!;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SamanSalamatResponse?> SubmitAnswerAsync(AnswerDto answerDto, HttpContext httpContext, CancellationToken cancellationToken = new())
    {
        var question = await _questionRepository.GetByIdAsync(answerDto.QuestionId, cancellationToken);

        if (question is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No question found with id of {answerDto.QuestionId}"
            };
        }

        var answer = _mapper.Map<Answer>(answerDto);

        answer.QuestionId = answerDto.QuestionId;

        var stringUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        int userId = int.Parse(stringUserId);

        var answeringUser = await _userRepository.GetByIdAsync(userId);

        var createdAnswer = await _answerRepository.CreateAsync(answer, cancellationToken);

        if (answeringUser is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "Internal Server Error - 500"
            };
        }

        answeringUser.Answers.Add(createdAnswer);

        answeringUser.Score += 1;

        _userRepository.UpdateAsync(answeringUser);

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

        if (answer is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No answer found with id of {answerId}"
            };
        }

        if (kind && answer.Question?.User is not null)
        {
            answer.Question.User.Score += 1;
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

    public new async Task<SamanSalamatResponse?> UpdateAsync(AnswerDto answerDto, CancellationToken cancellationToken = new())
    {
        var question = await _questionRepository.GetByIdAsync(answerDto.QuestionId, cancellationToken);

        if (question is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "No question found with given question ID"
            };
        }

        var answer = question.Answers?.SingleOrDefault(answer => answer.Id == answerDto.Id);

        if (answer is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "No answer found with given answer ID"
            };
        }

        _mapper.Map(answerDto, answer);
        
        var response = await _answerRepository.UpdateAsync(answer, cancellationToken);

        return new SamanSalamatResponse()
        {
            IsSuccess = true,
            Message = $"Successfully updated the answer. New answer title: {response.Title}, new answer description: {response.Description}"
        };
    }

    //public async Task<SamanSalamatResponse?> DeleteAsync(int answerId, CancellationToken cancellationToken = new())
    //{
    //    var answer = await _answerRepository.GetByIdAsync(answerId, cancellationToken);

    //    if (answer is null)
    //    {
    //        return new SamanSalamatResponse()
    //        {
    //            IsSuccess = false,
    //            Message = "No answer found with given answer id"
    //        };
    //    }

    //    return await DeleteAsync(answer, cancellationToken);

    //}

}