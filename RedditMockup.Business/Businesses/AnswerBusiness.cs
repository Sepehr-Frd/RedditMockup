using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RedditMockup.Business.Base;
using RedditMockup.Common.Dtos;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Business.Businesses;

public class AnswerBusiness : BaseBusiness<Answer, AnswerDto>
{
    private readonly AnswerRepository _answerRepository;
    private readonly QuestionRepository _questionRepository;
    private readonly QuestionBusiness _questionBusiness;
    private readonly AnswerVoteRepository _answerVoteRepository;
    private readonly UserRepository _userRepository;
    private readonly UserBusiness _userBusiness;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public AnswerBusiness(IUnitOfWork unitOfWork, IMapper mapper, UserBusiness userBusiness, QuestionBusiness questionBusiness) : base(unitOfWork, unitOfWork.AnswerRepository!, mapper)
    {
        _answerRepository = unitOfWork.AnswerRepository!;
        _questionRepository = unitOfWork.QuestionRepository!;
        _questionBusiness = questionBusiness;
        _answerVoteRepository = unitOfWork.AnswerVoteRepository!;
        _userBusiness = userBusiness;
        _userRepository = unitOfWork.UserRepository!;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<SamanSalamatResponse?> CreateAsync(AnswerDto dto, CancellationToken cancellationToken = new())
    {
        var question = await _questionBusiness.LoadModelByIdAsync(dto.QuestionId, cancellationToken);

        if (question is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No question found with id of {dto.QuestionId}"
            };
        }

        var answer = _mapper.Map<Answer>(dto);

        var user = await _userBusiness.LoadModelByIdAsync(dto.UserId);

        answer.QuestionId = question.Id;

        user!.Score += 1;

        await _userRepository.UpdateAsync(user, cancellationToken);

        return await CreateAsync(answer, cancellationToken);
    }

    private async Task<Answer?> LoadModelByIdAsync(int id, CancellationToken cancellationToken = new())
    {
        SieveModel sieveModel = new()
        {
            Filters = $"Id=={id}"
        };

        var answers = await _answerRepository.LoadAllAsync(sieveModel,
            include => include
                .Include(x => x.AnsweringUser)
                .Include(x => x.Question)
                .Include(x => x.Votes),
            cancellationToken);

        if (answers.Count == 0)
        {
            return null;
        }

        return answers.Single();
    }

    public override async Task<SamanSalamatResponse?> LoadByIdAsync(int id, CancellationToken cancellationToken = new())
    {

        var answer = await LoadModelByIdAsync(id, cancellationToken);

        if (answer is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No answer found with the ID of {id}"
            };
        }

        var response = _mapper.Map<AnswerDto>(answer);

        return new SamanSalamatResponse()
        {
            Data = response,
            IsSuccess = true
        };
    }

    public override async Task<SamanSalamatResponse?> UpdateAsync(int id, AnswerDto dto, CancellationToken cancellationToken = new())
    {
        var answer = await LoadModelByIdAsync(id, cancellationToken);

        if (answer is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "No answer found with given answer ID"
            };
        }

        _mapper.Map(dto, answer);

        return await UpdateAsync(answer, cancellationToken);

    }

    public override async Task<SamanSalamatResponse?> DeleteAsync(int id, CancellationToken cancellationToken = new())
    {
        var answer = await LoadModelByIdAsync(id, cancellationToken);

        if (answer is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No answer found with id of {id}"
            };
        }

        return await DeleteAsync(answer, cancellationToken);

    }

    public async Task<SamanSalamatResponse?> SubmitVoteAsync(int id, bool kind, CancellationToken cancellationToken = new())
    {
        var answer = await LoadModelByIdAsync(id, cancellationToken);

        if (answer is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No answer found with id of {id}"
            };
        }

        if (kind)
        {
            answer.Question!.User!.Score += 1;
        }

        var vote = new AnswerVote()
        {
            Kind = kind,
            AnswerId = answer.Id
        };

        await _answerVoteRepository.CreateAsync(vote, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new SamanSalamatResponse()
        {
            IsSuccess = true,
            Message = $"{(kind ? "Up" : "Down")}vote submitted"
        };
    }

}