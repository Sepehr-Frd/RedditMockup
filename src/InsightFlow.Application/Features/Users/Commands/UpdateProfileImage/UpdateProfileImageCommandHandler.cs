using Humanizer;
using InsightFlow.Application.Common;
using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Constants;
using InsightFlow.Common.Cqrs.Commands;
using InsightFlow.Domain.Common;
using InsightFlow.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InsightFlow.Application.Features.Users.Commands.UpdateProfileImage;

public class UpdateProfileImageCommandHandler : ICommandHandler<UpdateProfileImageCommand, DomainResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileImageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DomainResponse> HandleAsync(UpdateProfileImageCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetOneAsync(
            user => user.Uuid == request.Uuid,
            [user => user.ProfileImage],
            cancellationToken: cancellationToken);

        if (user is null)
        {
            var message = string.Format(
                StringConstants.EntityNotFoundByUuidTemplate,
                nameof(User).Humanize(LetterCasing.LowerCase),
                request.Uuid);

            return DomainResponse.CreateBaseFailure(message, StatusCodes.Status404NotFound);
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

        var profileImageBytes = memoryStream.ToArray();

        var imageFormat = imageFileExtension == ApplicationConstants.Jpg ? ApplicationConstants.Jpeg : imageFileExtension;

        user.ProfileImage!.ImageBytes = profileImageBytes;
        user.ProfileImage.ImageFormat = imageFormat;
        user.ProfileImage.PrepareForUpdate();
        user.PrepareForUpdate();

        await _unitOfWork.CommitChangesAsync(cancellationToken);

        return DomainResponse.CreateBaseSuccess(StringConstants.SuccessfulProfileImageUploadMessage, StatusCodes.Status200OK);
    }
}