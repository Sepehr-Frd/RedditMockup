using System.Net.Mime;
using Humanizer;
using InsightFlow.Api.Common.Dtos.Requests;
using InsightFlow.Application.Common;
using InsightFlow.Application.Features.BlogPosts.Commands.CreateBlogPost;
using InsightFlow.Application.Features.BlogPosts.Commands.DeleteBlogPost;
using InsightFlow.Application.Features.BlogPosts.Commands.UpdateBlogPost;
using InsightFlow.Application.Features.BlogPosts.Commands.UpdateCoverImage;
using InsightFlow.Application.Features.BlogPosts.Dtos;
using InsightFlow.Application.Features.BlogPosts.Queries.GetAllBlogPostsByFilter;
using InsightFlow.Application.Features.BlogPosts.Queries.GetAllBlogPostsCount;
using InsightFlow.Application.Features.BlogPosts.Queries.GetCoverImage;
using InsightFlow.Application.Features.BlogPosts.Queries.GetSingleBlogPost;
using InsightFlow.Application.Features.BlogPosts.Queries.GetUserBlogPosts;
using InsightFlow.Application.Features.Users.Queries.GetUserIdByUserUuid;
using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Constants;
using InsightFlow.Common.Cqrs;
using InsightFlow.Domain.Common;
using InsightFlow.Infrastructure.Common;
using InsightFlow.Infrastructure.Common.Constants;
using InsightFlow.Infrastructure.Common.Dtos;
using InsightFlow.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace InsightFlow.Api.Controllers;

[ApiController]
[Route("api/blog-posts")]
public class BlogPostController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;
    private readonly IDataValidator<PaginationDto> _paginationDtoValidator;

    public BlogPostController(
        IMediator mediator,
        IAuthService authService,
        IDataValidator<PaginationDto> paginationDtoValidator)
    {
        _mediator = mediator;
        _authService = authService;
        _paginationDtoValidator = paginationDtoValidator;
    }

    [HttpPost]
    [Authorize(InfrastructureConstants.UserPolicyName)]
    public async Task<ActionResult<DomainResponse<BlogPostResponseDto>>> CreateBlogPostAsync([FromBody] CreateBlogPostRequestDto request, CancellationToken cancellationToken)
    {
        var signedInUserUuid = _authService.GetSignedInUserUuid();

        var command = new CreateBlogPostCommand(
            request.Title,
            request.Body,
            Guid.Parse(signedInUserUuid));

        var result = await _mediator.SendAsync(command, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [Route("count")]
    [HttpGet]
    public async Task<ActionResult<PaginatedDomainResponse<IEnumerable<BlogPostResponseDto>>>> GetAllBlogPostsCountAsync(CancellationToken cancellationToken)
    {
        var request = new GetAllBlogPostsCountQuery();

        var response = await _mediator.SendAsync(request, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedDomainResponse<IEnumerable<BlogPostResponseDto>>>> GetAllBlogPostsByFilterAsync(
        [FromQuery] PaginationDto pagination,
        [FromQuery] string? title,
        [FromQuery] string? body,
        [FromQuery] Guid? authorUuid,
        CancellationToken cancellationToken)
    {
        var paginationDtoValidationResult = await _paginationDtoValidator.ValidateAsync(pagination, cancellationToken);

        if (!paginationDtoValidationResult.IsValid)
        {
            return BadRequest(PaginatedDomainResponse<IEnumerable<BlogPostResponseDto>>
                .CreateFailure(
                    string.Join(Environment.NewLine, paginationDtoValidationResult.ValidationErrors),
                    StatusCodes.Status400BadRequest));
        }

        var filterDto = new BlogPostFilterDto
        {
            Title = title,
            Body = body
        };

        if (authorUuid.HasValue)
        {
            var userRequest = new GetUserIdByUserUuidQuery(authorUuid.Value);

            var userIdResponse = await _mediator.SendAsync(userRequest, cancellationToken);

            if (!userIdResponse.IsSuccess)
            {
                var message = string.Format(
                    StringConstants.EntityNotFoundByUuidTemplate,
                    nameof(User).Humanize(LetterCasing.LowerCase),
                    authorUuid);

                return BadRequest(PaginatedDomainResponse<IEnumerable<BlogPostResponseDto>>
                    .CreateFailure(message, StatusCodes.Status400BadRequest));
            }

            filterDto.AuthorId = userIdResponse.Data;
        }

        var request = new GetAllBlogPostsByFilterQuery(filterDto, pagination.PageNumber, pagination.PageSize);

        var response = await _mediator.SendAsync(request, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Route("users/blog-posts")]
    [Authorize(InfrastructureConstants.UserPolicyName)]
    public async Task<ActionResult<PaginatedDomainResponse<IEnumerable<BlogPostResponseDto>>>> GetAllCurrentUserBlogPostsAsync(
        [FromQuery] PaginationDto pagination,
        CancellationToken cancellationToken)
    {
        var paginationDtoValidationResult = await _paginationDtoValidator.ValidateAsync(pagination, cancellationToken);

        if (!paginationDtoValidationResult.IsValid)
        {
            return BadRequest(PaginatedDomainResponse<IEnumerable<BlogPostResponseDto>>
                .CreateFailure(
                    string.Join(Environment.NewLine, paginationDtoValidationResult.ValidationErrors),
                    StatusCodes.Status400BadRequest));
        }

        var stringUuid = _authService.GetSignedInUserUuid();

        var request = new GetUserBlogPostsQuery(Guid.Parse(stringUuid), pagination.PageNumber, pagination.PageSize);

        var response = await _mediator.SendAsync(request, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Route("{uuid:guid}")]
    public async Task<ActionResult<DomainResponse<BlogPostResponseDto>>> GetSingleBlogPostAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        var request = new GetSingleBlogPostQuery(uuid);

        var response = await _mediator.SendAsync(request, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Route("{blogPostUuid:guid}/cover-image")]
    public async Task<IActionResult> GetCurrentUserProfileImageAsync([FromRoute] Guid blogPostUuid, CancellationToken cancellationToken)
    {
        var request = new GetCoverImageQuery(blogPostUuid);

        var response = await _mediator.SendAsync(request, cancellationToken);

        if (!response.IsSuccess)
        {
            return StatusCode(response.StatusCode, response);
        }

        var image = response.Data!;

        var imageName = blogPostUuid.ToString() + '-' + "cover-image";

        var contentType = image.ImageFormat switch
        {
            ApplicationConstants.Jpeg or ApplicationConstants.Jpg => MediaTypeNames.Image.Jpeg,
            ApplicationConstants.Png => MediaTypeNames.Image.Png,
            _ => MediaTypeNames.Application.Octet
        };

        var imageResult = new FileContentResult(image.ImageBytes, contentType)
        {
            FileDownloadName = imageName,
            LastModified = image.UpdatedAt
        };

        return imageResult;
    }

    [HttpPut]
    [Authorize]
    [Route("{blogPostUuid:guid}/cover-image")]
    public async Task<ActionResult<BlogPostResponseDto>> UpdateUserProfileImageAsync(
        IFormFile imageFile,
        [FromRoute] Guid blogPostUuid,
        CancellationToken cancellationToken)
    {
        var signedInUserUuid = _authService.GetSignedInUserUuid();

        var file = new FormFileAdapter(imageFile);

        var command = new UpdateCoverImageCommand(Guid.Parse(signedInUserUuid), blogPostUuid, file);

        var response = await _mediator.SendAsync(command, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch]
    [Authorize]
    public async Task<ActionResult<DomainResponse<BlogPostResponseDto>>> UpdateBlogPostAsync([FromBody] UpdateBlogPostRequestDto request, CancellationToken cancellationToken)
    {
        var signedInUserUuid = _authService.GetSignedInUserUuid();

        var command = new UpdateBlogPostCommand(
            Guid.Parse(signedInUserUuid),
            request.BlogPostUuid,
            request.NewTitle,
            request.NewBody);

        var response = await _mediator.SendAsync(command, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete]
    [Authorize]
    [Route("{uuid:guid}")]
    public async Task<ActionResult<DomainResponse<BlogPostResponseDto>>> DeleteBlogPostByUuidAsync([FromRoute] Guid uuid, CancellationToken cancellationToken)
    {
        var signedInUserUuid = _authService.GetSignedInUserUuid();

        var command = new DeleteBlogPostCommand(
            uuid,
            Guid.Parse(signedInUserUuid));

        var response = await _mediator.SendAsync(command, cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpOptions]
    public IActionResult BlogPostsOption()
    {
        Response
            .Headers
            .Add(new KeyValuePair<string, StringValues>("Allow", $"{HttpMethods.Post},{HttpMethods.Get},{HttpMethods.Patch},{HttpMethods.Put},{HttpMethods.Delete}"));

        return Ok();
    }
}