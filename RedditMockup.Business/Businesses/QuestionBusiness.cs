using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RedditMockup.Business.Base;
using RedditMockup.Common.Dtos;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Business.Businesses;

public class QuestionBusiness : BaseBusiness<Question, QuestionDto>
{
    private readonly QuestionRepository _questionRepository;
    private readonly QuestionVoteRepository _questionVoteRepository;
    private readonly UserRepository _userRepository;
    private readonly UserBusiness _userBusiness;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QuestionBusiness(IUnitOfWork unitOfWork, IMapper mapper, UserBusiness userBusiness) : base(unitOfWork, unitOfWork.QuestionRepository!, mapper)
    {
        _questionRepository = unitOfWork.QuestionRepository!;
        _questionVoteRepository = unitOfWork.QuestionVoteRepository!;
        _userRepository = unitOfWork.UserRepository!;
        _userBusiness = userBusiness;
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }

    public async Task<SamanSalamatResponse?> CreateAsync(QuestionDto dto, CancellationToken cancellationToken = new())
    {
        var question = _mapper.Map<Question>(dto);

        var user = await _userBusiness.LoadModelByIdAsync(dto.UserId, cancellationToken);
        
        question.UserId = user!.Id;

        user.Score += 1;

        return await CreateAsync(question, cancellationToken);
    }

    public async Task<Question?> LoadModelByIdAsync(int id, CancellationToken cancellationToken = new())
    {
        SieveModel sieveModel = new()
        {
            Filters = $"Id=={id}"
        };

        var questions = await _questionRepository.LoadAllAsync(sieveModel,
            include => include
            .Include(x => x.User)
            .Include(x => x.Votes)
            .Include(x => x.Answers),
            cancellationToken);

        if (questions.Count == 0)
        {
            return null;
        }

        return questions.Single();
    }

    public async Task<SamanSalamatResponse?> LoadByIdAsync(int id, CancellationToken cancellationToken = new())
    {
        var question = await LoadModelByIdAsync(id, cancellationToken);

        if (question is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No question found with the ID of {id}"
            };
        }

        var response = _mapper.Map<AnswerDto>(question);

        return new SamanSalamatResponse()
        {
            Data = response,
            IsSuccess = true
        };
    }

    public async Task<SamanSalamatResponse?> LoadAnswersAsync(int id, CancellationToken cancellationToken = new())
    {
        var question = await LoadModelByIdAsync(id, cancellationToken);

        if (question is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "No question found with given question ID"
            };
        }
        
        var answers = question.Answers!.ToList();

        var response = _mapper.Map<List<AnswerDto>>(answers);

        return new SamanSalamatResponse()
        {
            Data = response,
            IsSuccess = true
        };
    }

    public async Task<SamanSalamatResponse?> LoadVotesAsync(int id, CancellationToken cancellationToken = new())
    {
        var question = await LoadModelByIdAsync(id, cancellationToken);

        if (question is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "No question found with given question ID"
            };
        }

        var votes = question.Votes!.ToList();
        
        var response = _mapper.Map<List<VoteDto>>(votes);

        return new SamanSalamatResponse()
        {
            Data = response,
            IsSuccess = true,
        };

    }

    public async Task<SamanSalamatResponse?> SubmitVoteAsync(int id, bool kind, CancellationToken cancellationToken = new())
    {
        var question = await LoadModelByIdAsync(id, cancellationToken);

        if (question is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No question found with id of {id}"
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

        await _questionVoteRepository.CreateAsync(vote, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new SamanSalamatResponse()
        {
            IsSuccess = true,
            Message = $"{(kind ? "Up" : "Down")}vote submitted"
        };

    }

    public async Task<SamanSalamatResponse?> UpdateAsync(int id, QuestionDto questionDto, CancellationToken cancellationToken = new())
    {
        var question = await LoadModelByIdAsync(id, cancellationToken);

        if (question is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "No question found with given question ID"
            };
        }

        _mapper.Map(questionDto, question);

        return await UpdateAsync(question, cancellationToken);

    }

    public async Task<SamanSalamatResponse?> DeleteAsync(int id, CancellationToken cancellationToken = new())
    {
        var question = await LoadModelByIdAsync(id, cancellationToken);

        if (question is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No question found with id of {id}"
            };
        }

        return await DeleteAsync(question, cancellationToken);

    }

}