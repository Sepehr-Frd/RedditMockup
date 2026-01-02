using Humanizer;
using InsightFlow.Application.Common;
using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Constants;
using InsightFlow.Common.Cqrs.Commands;
using InsightFlow.Domain.Common;
using InsightFlow.Domain.Entities;
using InsightFlow.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace InsightFlow.Application.Features.BlogPosts.Commands.UpdateCoverImage;

public class UpdateCoverImageCommandHandler : ICommandHandler<UpdateCoverImageCommand, DomainResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCoverImageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DomainResponse> HandleAsync(UpdateCoverImageCommand request, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetOneAsync(
            user => user.Uuid == request.UserUuid,
            cancellationToken: cancellationToken);

        if (user is null)
        {
            var message = string.Format(
                StringConstants.EntityNotFoundByUuidTemplate,
                nameof(User).Humanize(LetterCasing.LowerCase),
                request.UserUuid);

            return DomainResponse.CreateBaseFailure(message, StatusCodes.Status404NotFound);
        }

        var blogPost = await _unitOfWork.BlogPostRepository.GetOneAsync(
            post => post.Uuid == request.BlogPostUuid,
            cancellationToken: cancellationToken);

        if (blogPost is null)
        {
            var message = string.Format(
                StringConstants.EntityNotFoundByUuidTemplate,
                nameof(BlogPost).Humanize(LetterCasing.LowerCase),
                request.BlogPostUuid);

            return DomainResponse.CreateBaseFailure(message, StatusCodes.Status404NotFound);
        }

        if (blogPost.AuthorId != user.Id)
        {
            var forbiddenMessage = string.Format(
                StringConstants.ForbiddenActionTemplate,
                StringConstants.UpdateActionName.Humanize(LetterCasing.LowerCase),
                nameof(BlogPost).Humanize(LetterCasing.LowerCase));

            return DomainResponse.CreateBaseFailure(forbiddenMessage, StatusCodes.Status403Forbidden);
        }

        var currentImage = await _unitOfWork
            .ImageRepository
            .GetOneAsync(
                image => image.Type == ImageType.BlogPostCoverImage && image.OwnerId == blogPost.Id,
                cancellationToken: cancellationToken);

        if (currentImage is not null)
        {
            _unitOfWork.ImageRepository.Delete(currentImage);
        }

        var imageFileExtension = Path.GetExtension(request.ImageFile.FileName).ToLowerInvariant().TrimStart('.');

        if (string.IsNullOrEmpty(imageFileExtension) || !ApplicationConstants.ValidProfileImageFormats.Contains(imageFileExtension))
        {
            var message = string.Format(
                StringConstants.InvalidProfileImageFormatMessage,
                imageFileExtension,
                string.Join(", ", ApplicationConstants.Jpeg, ApplicationConstants.Png));

            return DomainResponse.CreateBaseFailure(message, StatusCodes.Status400BadRequest);
        }

        await using var memoryStream = new MemoryStream();

        await request.ImageFile.OpenReadStream().CopyToAsync(memoryStream, cancellationToken);

        var imageBytes = memoryStream.ToArray();

        var imageFormat = imageFileExtension == ApplicationConstants.Jpg ? ApplicationConstants.Jpeg : imageFileExtension;

        var image = new Image
        {
            ImageBytes = imageBytes,
            ImageFormat = imageFormat,
            OwnerId = blogPost.Id,
            Type = ImageType.BlogPostCoverImage
        };

        await _unitOfWork.ImageRepository.CreateAsync(image, cancellationToken);

        blogPost.PrepareForUpdate();

        await _unitOfWork.CommitChangesAsync(cancellationToken);

        return DomainResponse.CreateBaseSuccess(StringConstants.SuccessfulBlogPostCoverImageUploadMessage, StatusCodes.Status200OK);
    }
}