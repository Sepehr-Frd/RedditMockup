using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using RedditMockup.Business.Base;
using RedditMockup.Common.Dtos;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;

namespace RedditMockup.Business.Businesses;

public class QuestionBusiness : BaseBusiness<Question, QuestionDto>
{
    private readonly QuestionRepository _questionRepository;
    private readonly QuestionVoteRepository _questionVoteRepository;
    private readonly UserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QuestionBusiness(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, unitOfWork.QuestionRepository!, mapper)
    {
        _questionRepository = unitOfWork.QuestionRepository!;
        _questionVoteRepository = unitOfWork.QuestionVoteRepository!;
        _userRepository = unitOfWork.UserRepository!;
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }

    public async Task<SamanSalamatResponse?> CreateAsync(QuestionDto dto, HttpContext httpContext, CancellationToken cancellationToken = new())
    {
        var question = _mapper.Map<Question>(dto);

        var stringUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        int userId = int.Parse(stringUserId);

        var user = await _userRepository.GetByIdAsync(userId);


        var questionInstance = await _questionRepository.CreateAsync(question, cancellationToken);

        user.Questions.Add(questionInstance);

        user.Score += 1;

        _userRepository.UpdateAsync(user);

        await _unitOfWork.CommitAsync(cancellationToken);

        var result = _mapper.Map<QuestionDto>(questionInstance);

        return new SamanSalamatResponse
        {
            Data = result,
            IsSuccess = true,
            Message = "Entity Saved"
        };
    }

    public async Task<QuestionDto?> GetByIdAsync(int questionId, CancellationToken cancellationToken = new())
    {
        var question = await _questionRepository.GetByIdAsync(questionId, cancellationToken);
        var dto = _mapper.Map<QuestionDto>(question);
        return dto;
    }

    public async Task<List<AnswerDto>?> GetAnswersAsync(int questionId, CancellationToken cancellationToken = new())
    {
        var question = await _questionRepository.GetByIdAsync(questionId, cancellationToken);
        var answers = question.Answers.ToList();
        var response = _mapper.Map<List<AnswerDto>>(answers);

        return response;

    }

    public async Task<List<VoteDto>?> GetVotesAsync(int questionId, CancellationToken cancellationToken = new())
    {
        var question = await _questionRepository.GetByIdAsync(questionId, cancellationToken);
        var votes = question.Votes.ToList();
        var response = _mapper.Map<List<VoteDto>>(votes);

        return response;

    }

    public async Task<SamanSalamatResponse?> SubmitVoteAsync(int questionId, bool kind, CancellationToken cancellationToken = new())
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

        if (kind)
        {
            question.User!.Score += 1;
        }

        var vote = new QuestionVote()
        {
            Kind = kind,
            QuestionId = question.Id
        };

        var createdVote = await _questionVoteRepository.CreateAsync(vote, cancellationToken);

        question.Votes.Add(createdVote);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new SamanSalamatResponse()
        {
            IsSuccess = true,
            Message = "Vote submitted"
        };

    }
}