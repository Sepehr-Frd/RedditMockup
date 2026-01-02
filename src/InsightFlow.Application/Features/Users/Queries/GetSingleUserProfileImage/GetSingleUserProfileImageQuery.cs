using InsightFlow.Application.Features.Users.Dtos;
using InsightFlow.Common.Cqrs.Queries;
using InsightFlow.Domain.Common;

namespace InsightFlow.Application.Features.Users.Queries.GetSingleUserProfileImage;

public record GetSingleUserProfileImageQuery(Guid UserUuid) : IQuery<DomainResponse<ImageResponseDto>>;