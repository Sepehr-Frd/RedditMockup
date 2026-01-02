using Humanizer;
using InsightFlow.Application.Features.Users.Dtos;
using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Constants;
using InsightFlow.Common.Cqrs.Commands;
using InsightFlow.Domain.Common;
using InsightFlow.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InsightFlow.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, DomainResponse<UserResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMappingService _mappingService;
    private readonly IRoleService _roleService;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IMappingService mappingService,
        IRoleService roleService,
        ILogger<CreateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mappingService = mappingService;
        _roleService = roleService;
        _logger = logger;
    }

    public async Task<DomainResponse<UserResponseDto>> HandleAsync(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        var isEmailUnique = await _unitOfWork.UserRepository.IsEmailUniqueAsync(request.Email, cancellationToken);

        if (!isEmailUnique)
        {
            var message = string.Format(
                StringConstants.PropertyNotUniqueTemplate,
                request.Email,
                nameof(CreateUserCommand.Email).Humanize(LetterCasing.LowerCase));

            return DomainResponse<UserResponseDto>.CreateFailure(message, StatusCodes.Status400BadRequest);
        }

        var isUsernameUnique = await _unitOfWork.UserRepository.IsUsernameUniqueAsync(request.Username, cancellationToken);

        if (!isUsernameUnique)
        {
            var message = string.Format(
                StringConstants.PropertyNotUniqueTemplate,
                request.Username,
                nameof(CreateUserCommand.Username).Humanize(LetterCasing.LowerCase));

            return DomainResponse<UserResponseDto>.CreateFailure(message, StatusCodes.Status400BadRequest);
        }

        var userEntity = _mappingService.Map<CreateUserCommand, User>(request);

        if (userEntity is null)
        {
            _logger.LogCritical(StringConstants.MappingErrorLogTemplate, typeof(CreateUserCommand), typeof(User));

            return DomainResponse<UserResponseDto>.CreateFailure(
                StringConstants.InternalServerError,
                StatusCodes.Status500InternalServerError);
        }

        var userRoleId = _roleService.GetRoleIdByRoleTitle(DomainConstants.BasicUserRoleTitle);

        if (!userRoleId.HasValue)
        {
            _logger.LogCritical(StringConstants.UnsuccessfulRoleIdRetrieval, DomainConstants.BasicUserRoleTitle);

            return DomainResponse<UserResponseDto>.CreateFailure(
                StringConstants.InternalServerError,
                StatusCodes.Status500InternalServerError);
        }

        userEntity.UserRoles.Add(new UserRole
        {
            UserId = userEntity.Id,
            RoleId = userRoleId.Value
        });

        if (request.AdditionalRoles?.Length > 0)
        {
            foreach (var roleTitle in request.AdditionalRoles)
            {
                var roleId = _roleService.GetRoleIdByRoleTitle(roleTitle);

                if (!roleId.HasValue)
                {
                    var message = string.Format(
                        StringConstants.InvalidParametersTemplate,
                        $"{nameof(CreateUserCommand.AdditionalRoles).Humanize(LetterCasing.LowerCase)}({roleTitle})");

                    return DomainResponse<UserResponseDto>.CreateFailure(message, StatusCodes.Status400BadRequest);
                }

                userEntity.UserRoles.Add(new UserRole
                {
                    UserId = userEntity.Id,
                    RoleId = roleId.Value
                });
            }
        }

        await _unitOfWork.UserRepository.CreateAsync(userEntity, cancellationToken);

        var commitResult = await _unitOfWork.CommitChangesAsync(cancellationToken);

        if (commitResult < 1)
        {
            _logger.LogCritical(StringConstants.DatabasePersistenceErrorLogTemplate, typeof(User), StringConstants.CreateActionName);

            return DomainResponse<UserResponseDto>.CreateFailure(
                StringConstants.InternalServerError,
                StatusCodes.Status500InternalServerError);
        }

        var userResponseDto = _mappingService.Map<User, UserResponseDto>(userEntity);

        if (userResponseDto is not null)
        {
            return DomainResponse<UserResponseDto>.CreateSuccess(null, StatusCodes.Status201Created, userResponseDto);
        }

        _logger.LogCritical(StringConstants.MappingErrorLogTemplate, typeof(User), typeof(UserResponseDto));

        return DomainResponse<UserResponseDto>.CreateFailure(
            StringConstants.InternalServerError,
            StatusCodes.Status500InternalServerError);
    }
}