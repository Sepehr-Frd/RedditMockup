using InsightFlow.Application.Interfaces;
using InsightFlow.Common.Cqrs.Commands;
using InsightFlow.Domain.Common;

namespace InsightFlow.Application.Features.Users.Commands.UpdateProfileImage;

public record UpdateProfileImageCommand(Guid Uuid, IFile ImageFile) : ICommand<DomainResponse>;