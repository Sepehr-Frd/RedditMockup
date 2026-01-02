namespace InsightFlow.Application.Features.Users.Dtos;

public record ImageResponseDto(byte[] ImageBytes, string ImageFormat, DateTime UpdatedAt);