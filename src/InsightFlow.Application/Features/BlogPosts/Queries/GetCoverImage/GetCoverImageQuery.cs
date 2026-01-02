using InsightFlow.Application.Features.Users.Dtos;
using InsightFlow.Common.Cqrs.Queries;
using InsightFlow.Domain.Common;

namespace InsightFlow.Application.Features.BlogPosts.Queries.GetCoverImage;

public record GetCoverImageQuery(Guid BlogPostUuid) : IQuery<DomainResponse<ImageResponseDto>>;