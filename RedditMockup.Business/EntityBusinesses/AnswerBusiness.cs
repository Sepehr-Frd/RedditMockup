﻿using Microsoft.IdentityModel.Tokens;
using RedditMockup.Business.Base;
using RedditMockup.Common.Dtos;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;
using Sieve.Models;
using System.Net;

namespace RedditMockup.Business.EntityBusinesses;

public class AnswerBusiness : BaseBusiness<Answer>
{
    #region [Fields]

    private readonly AnswerVoteRepository _answerVoteRepository;

    private readonly AnswerRepository _answerRepository;

    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region [Constructor]

    public AnswerBusiness(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.AnswerRepository!)
    {
        _unitOfWork = unitOfWork;
        _answerRepository = unitOfWork.AnswerRepository!;
        _answerVoteRepository = unitOfWork.AnswerVoteRepository!;
    }

    #endregion

    #region [Methods]

    public async Task<CustomResponse<IEnumerable<Answer>>> LoadAnswersByQuestionIdAsync(int questionId, CancellationToken cancellationToken = new())
    {

        SieveModel sieveModel = new()
        {
            Filters = $"QuestionId=={questionId}"
        };

        var answers = await LoadAllAsync(sieveModel, cancellationToken);

        if (answers.IsNullOrEmpty())
        {
            return new()
            {
                IsSuccess = false,
                Message = $"No question found with ID of {questionId}",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }

        return new()
        {
            Data = answers,
            IsSuccess = true,
            HttpStatusCode = HttpStatusCode.OK
        };
    }

    public async Task<CustomResponse?> SubmitVoteAsync(int id, bool kind, CancellationToken cancellationToken = new())
    {
        var answer = await LoadByIdAsync(id, cancellationToken);

        if (answer is null)
        {
            return new CustomResponse
            {
                IsSuccess = false,
                Message = $"No answer found with ID of {id}",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }

        if (kind)
        {
            answer.User!.Score += 1;
        }

        var vote = new AnswerVote
        {
            Kind = kind,
            AnswerId = answer.Id
        };

        await _answerVoteRepository.CreateAsync(vote, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new CustomResponse
        {
            IsSuccess = true,
            Message = $"{(kind ? "Up" : "Down")}vote submitted",
            HttpStatusCode = HttpStatusCode.OK
        };
    }

    public async Task<CustomResponse<IEnumerable<AnswerVote>>> LoadVotesAsync(int id, CancellationToken cancellationToken = new())
    {
        var answer = await _answerRepository.LoadByIdAsync(id, cancellationToken);

        if (answer is null)
        {
            return new()
            {
                IsSuccess = false,
                Message = $"No answer found with ID of {id}",
                HttpStatusCode = HttpStatusCode.NotFound
            };
        }

        var votes = answer.Votes!.ToList();

        return new()
        {
            Data = votes,
            IsSuccess = true,
            HttpStatusCode = HttpStatusCode.OK
        };
    }

    #endregion
}