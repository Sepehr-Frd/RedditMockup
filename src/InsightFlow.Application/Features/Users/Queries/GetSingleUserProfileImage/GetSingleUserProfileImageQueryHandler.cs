using Humanizer;
using InsightFlow.Application.Features.Users.Dtos;
using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Constants;
using InsightFlow.Common.Cqrs.Queries;
using InsightFlow.Domain.Common;
using InsightFlow.Domain.Entities;
using InsightFlow.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace InsightFlow.Application.Features.Users.Queries.GetSingleUserProfileImage;

public class GetSingleUserProfileImageQueryHandler : IQueryHandler<GetSingleUserProfileImageQuery, DomainResponse<ImageResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSingleUserProfileImageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DomainResponse<ImageResponseDto>> HandleAsync(GetSingleUserProfileImageQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetOneAsync(
            user => user.Uuid == request.UserUuid,
            disableTracking: true,
            cancellationToken: cancellationToken);

        if (user is null)
        {
            var message = string.Format(
                StringConstants.EntityNotFoundByUuidTemplate,
                nameof(User).Humanize(LetterCasing.LowerCase),
                request.UserUuid);

            return DomainResponse<ImageResponseDto>.CreateFailure(message, StatusCodes.Status404NotFound);
        }

        var profileImage = await _unitOfWork
            .ImageRepository
            .GetOneAsync(
                image => image.Type == ImageType.ProfileImage && image.OwnerId == user.Id,
                cancellationToken: cancellationToken);

        if (profileImage?.ImageBytes is null)
        {
            return DomainResponse<ImageResponseDto>.CreateFailure(StringConstants.NoProfileImageUploadedYetMessage, StatusCodes.Status404NotFound);
        }

        var responseDto = new ImageResponseDto(
            profileImage.ImageBytes,
            profileImage.ImageFormat!,
            profileImage.UpdatedAt);

        return DomainResponse<ImageResponseDto>.CreateSuccess(null, StatusCodes.Status200OK, responseDto);
    }
}