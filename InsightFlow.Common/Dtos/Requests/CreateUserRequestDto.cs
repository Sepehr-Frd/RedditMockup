namespace InsightFlow.Common.Dtos.Requests;

public record CreateUserRequestDto(string Username, string Password, string FirstName, string? LastName, string Email, string? Bio = null);