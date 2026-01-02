using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Cqrs.Commands;
using InsightFlow.Domain.Common;

namespace InsightFlow.Application.Features.BlogPosts.Commands.UpdateCoverImage;

public record UpdateCoverImageCommand(Guid UserUuid, Guid BlogPostUuid, IFile ImageFile) : ICommand<DomainResponse>;