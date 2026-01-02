using System.Net.Mime;
using InsightFlow.Api.Common.Dtos.Requests;
using InsightFlow.Application.Common;
using InsightFlow.Application.Features.Users.Commands.CreateUser;
using InsightFlow.Application.Features.Users.Commands.UpdateProfileImage;
using InsightFlow.Application.Features.Users.Commands.UpdateUser;
using InsightFlow.Application.Features.Users.Dtos;
using InsightFlow.Application.Features.Users.Queries.GetSingleUser;
using InsightFlow.Application.Features.Users.Queries.GetSingleUserProfileImage;
using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Cqrs;
using InsightFlow.Domain.Common;
using InsightFlow.Infrastructure.Common;
using InsightFlow.Infrastructure.Common.Constants;
using InsightFlow.Infrastructure.Common.Dtos;
using InsightFlow.Infrastructure.Common.Helpers;
using InsightFlow.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace InsightFlow.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDataValidator<CreateUserRequestDto> _dataValidator;
    private readonly IAuthService _authService;

    public UserController(
        IMediator mediator,
        IDataValidator<CreateUserRequestDto> dataValidator,
        IAuthService authService)
    {
        _mediator = mediator;
        _dataValidator = dataValidator;
        _authService = authService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<DomainResponse<UserResponseDto>>> CreateUserAsync([FromBody] CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken)
    {
        var dataValidationResult = await _dataValidator.ValidateAsync(createUserRequestDto, cancellationToken);

        if (!dataValidationResult.IsValid)
        {
            return BadRequest(DomainResponse<UserResponseDto>
                .CreateFailure(
                    string.Join(Environment.NewLine, dataValidationResult.ValidationErrors),
                    StatusCodes.Status400BadRequest));
        }

        if (_authService.IsSignedIn())
        {
            return Forbid();
        }

        var command = new CreateUserCommand(
            createUserRequestDto.Username,
            PasswordHelper.HashPassword(createUserRequestDto.Password),
            createUserRequestDto.Email,
            createUserRequestDto.FirstName,
            createUserRequestDto.LastName);

        var result = await _mediator.SendAsync(command, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    [Route("admins")]
    [Authorize(InfrastructureConstants.AdminPolicyName)]
    public async Task<ActionResult<DomainResponse<UserResponseDto>>> CreateAdminAsync([FromBody] CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken)
    {
        var dataValidationResult = await _dataValidator.ValidateAsync(createUserRequestDto, cancellationToken);

        if (!dataValidationResult.IsValid)
        {
            return BadRequest(DomainResponse<UserResponseDto>
                .CreateFailure(
                    string.Join(Environment.NewLine, dataValidationResult.ValidationErrors),
                    StatusCodes.Status400BadRequest));
        }

        var command = new CreateUserCommand(
            createUserRequestDto.Username,
            PasswordHelper.HashPassword(createUserRequestDto.Password),
            createUserRequestDto.Email,
            createUserRequestDto.FirstName,
            createUserRequestDto.LastName,
            [DomainConstants.AdminRoleTitle]);

        var result = await _mediator.SendAsync(command, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    [Authorize]
    [Route("information")]
    public async Task<ActionResult<UserResponseDto>> GetCurrentUserInformationAsync(CancellationToken cancellationToken)
    {
        var signedInUserUuid = _authService.GetSignedInUserUuid();

        var request = new GetSingleUserQuery(Guid.Parse(signedInUserUuid));

        var response = await _mediator.SendAsync(request, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Authorize]
    [Route("profile-image")]
    public async Task<IActionResult> GetCurrentUserProfileImageAsync(CancellationToken cancellationToken)
    {
        var signedInUserUuid = _authService.GetSignedInUserUuid();

        var request = new GetSingleUserProfileImageQuery(Guid.Parse(signedInUserUuid));

        var response = await _mediator.SendAsync(request, cancellationToken);

        if (!response.IsSuccess)
        {
            return StatusCode(response.StatusCode, response);
        }

        var profileImage = response.Data!;

        var imageName = signedInUserUuid + '-' + "profile-picture";

        var contentType = profileImage.ImageFormat switch
        {
            ApplicationConstants.Jpeg or ApplicationConstants.Jpg => MediaTypeNames.Image.Jpeg,
            ApplicationConstants.Png => MediaTypeNames.Image.Png,
            _ => MediaTypeNames.Application.Octet
        };

        var profileImageFile = new FileContentResult(profileImage.ImageBytes, contentType)
        {
            FileDownloadName = imageName,
            LastModified = profileImage.UpdatedAt
        };

        return profileImageFile;
    }

    [HttpPatch]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> UpdateUserAsync([FromBody] UpdateUserInformationRequestDto request, CancellationToken cancellationToken)
    {
        var signedInUserUuid = _authService.GetSignedInUserUuid();

        var command = new UpdateUserCommand(
            Guid.Parse(signedInUserUuid),
            request.NewEmail,
            request.NewFirstName,
            request.NewLastName);

        var response = await _mediator.SendAsync(command, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    [Authorize]
    [Route("profile-image")]
    public async Task<ActionResult<UserResponseDto>> UpdateUserProfileImageAsync(IFormFile imageFile, CancellationToken cancellationToken)
    {
        var signedInUserUuid = _authService.GetSignedInUserUuid();

        var file = new FormFileAdapter(imageFile);

        var command = new UpdateProfileImageCommand(Guid.Parse(signedInUserUuid), file);

        var response = await _mediator.SendAsync(command, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpOptions]
    public IActionResult UsersOption()
    {
        Response
            .Headers
            .Add(new KeyValuePair<string, StringValues>("Allow", $"{HttpMethods.Post},{HttpMethods.Get},{HttpMethods.Patch},{HttpMethods.Put},{HttpMethods.Delete}"));

        return Ok();
    }
}