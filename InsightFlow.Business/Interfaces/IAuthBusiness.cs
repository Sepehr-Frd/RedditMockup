using InsightFlow.Common.Dtos;
using InsightFlow.Common.Dtos.CustomResponses;

namespace InsightFlow.Business.Interfaces;

public interface IAuthBusiness
{
    string GetSignedInUserExternalId();

    Task<CustomResponse<string>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);

}