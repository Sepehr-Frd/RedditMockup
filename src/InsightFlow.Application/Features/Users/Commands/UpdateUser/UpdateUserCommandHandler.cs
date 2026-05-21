using Humanizer;
using InsightFlow.Application.Features.Users.Commands.CreateUser;
using InsightFlow.Application.Features.Users.Dtos;
using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Constants;
using InsightFlow.Common.Cqrs.Commands;
using InsightFlow.Domain.Common;
using InsightFlow.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InsightFlow.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, DomainResponse<UserResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMappingService _mappingService;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IMappingService mappingService,
        ILogger<UpdateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mappingService = mappingService;
        _logger = logger;
    }

    public async Task<DomainResponse<UserResponseDto>> HandleAsync(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetOneAsync(
            user => user.Uuid == request.Uuid,
            cancellationToken: cancellationToken);

        if (user is null)
        {
            var message = string.Format(
                StringConstants.EntityNotFoundByUuidTemplate,
                nameof(User).Humanize(LetterCasing.LowerCase),
                request.Uuid);

            return DomainResponse<UserResponseDto>.CreateFailure(message, StatusCodes.Status404NotFound);
        }

        if ((string.IsNullOrWhiteSpace(request.NewFirstName) || request.NewFirstName == user.FirstName) &&
            (string.IsNullOrWhiteSpace(request.NewLastName) || request.NewLastName == user.LastName) &&
            (string.IsNullOrWhiteSpace(request.NewEmail) || request.NewEmail.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase)))
        {
            return DomainResponse<UserResponseDto>.CreateFailure(
                StringConstants.IdenticalNewPropertyValuesTemplate,
                StatusCodes.Status400BadRequest);
        }

        if (!string.IsNullOrWhiteSpace(request.NewEmail) && request.NewEmail != user.Email)
        {
            var isEmailUnique = await _unitOfWork.UserRepository.IsEmailUniqueAsync(request.NewEmail, cancellationToken);

            if (!isEmailUnique)
            {
                var message = string.Format(
                    StringConstants.PropertyNotUniqueTemplate,
                    request.NewEmail,
                    nameof(CreateUserCommand.Email).Humanize(LetterCasing.LowerCase));

                return DomainResponse<UserResponseDto>.CreateFailure(message, StatusCodes.Status400BadRequest);
            }

            user.Email = request.NewEmail;
        }

        if (!string.IsNullOrWhiteSpace(request.NewFirstName) && request.NewFirstName != user.FirstName)
        {
            user.FirstName = request.NewFirstName;
        }

        if (!string.IsNullOrWhiteSpace(request.NewLastName) && request.NewLastName != user.LastName)
        {
            user.LastName = request.NewLastName;
        }

        user.PrepareForUpdate();

        var commitResult = await _unitOfWork.CommitChangesAsync(cancellationToken);

        if (commitResult < 1)
        {
            _logger.LogCritical(StringConstants.DatabasePersistenceErrorLogTemplate, typeof(User), StringConstants.UpdateActionName);

            return DomainResponse<UserResponseDto>.CreateFailure(
                StringConstants.InternalServerError,
                StatusCodes.Status500InternalServerError);
        }

        var userResponseDto = _mappingService.Map<User, UserResponseDto>(user);

        if (userResponseDto is null)
        {
            _logger.LogCritical(StringConstants.MappingErrorLogTemplate, typeof(User), typeof(UserResponseDto));

            return DomainResponse<UserResponseDto>.CreateFailure(
                StringConstants.InternalServerError,
                StatusCodes.Status500InternalServerError);
        }

        var successMessage = string.Format(StringConstants.SuccessfulUpdateTemplate, nameof(User).Humanize(LetterCasing.LowerCase));

        return DomainResponse<UserResponseDto>.CreateSuccess(successMessage, StatusCodes.Status200OK, userResponseDto);
    }
}