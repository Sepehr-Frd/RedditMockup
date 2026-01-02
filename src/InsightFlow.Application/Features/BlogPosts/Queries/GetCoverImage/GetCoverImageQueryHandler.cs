using Humanizer;
using InsightFlow.Application.Features.Users.Dtos;
using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Constants;
using InsightFlow.Common.Cqrs.Queries;
using InsightFlow.Domain.Common;
using InsightFlow.Domain.Entities;
using InsightFlow.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace InsightFlow.Application.Features.BlogPosts.Queries.GetCoverImage;

public class GetCoverImageQueryHandler : IQueryHandler<GetCoverImageQuery, DomainResponse<ImageResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCoverImageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DomainResponse<ImageResponseDto>> HandleAsync(GetCoverImageQuery request, CancellationToken cancellationToken = default)
    {
        var blogPost = await _unitOfWork.BlogPostRepository.GetOneAsync(
            post => post.Uuid == request.BlogPostUuid,
            cancellationToken: cancellationToken);

        if (blogPost is null)
        {
            var message = string.Format(
                StringConstants.EntityNotFoundByUuidTemplate,
                nameof(BlogPost).Humanize(LetterCasing.LowerCase),
                request.BlogPostUuid);

            return DomainResponse<ImageResponseDto>.CreateFailure(message, StatusCodes.Status404NotFound);
        }

        var image = await _unitOfWork
            .ImageRepository
            .GetOneAsync(
                image => image.Type == ImageType.BlogPostCoverImage && image.OwnerId == blogPost.Id,
                cancellationToken: cancellationToken);

        if (image?.ImageBytes is null)
        {
            return DomainResponse<ImageResponseDto>.CreateFailure(StringConstants.NoBlogPostCoverImageUploadedYetMessage, StatusCodes.Status404NotFound);
        }

        var responseDto = new ImageResponseDto(
            image.ImageBytes,
            image.ImageFormat!,
            image.UpdatedAt);

        return DomainResponse<ImageResponseDto>.CreateSuccess(null, StatusCodes.Status200OK, responseDto);
    }
}