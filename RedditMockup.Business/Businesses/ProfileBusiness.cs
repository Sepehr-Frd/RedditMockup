using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using RedditMockup.Business.Base;
using RedditMockup.Common.Dtos;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;

namespace RedditMockup.Business.Businesses;

public class ProfileBusiness : BaseBusiness<Model.Entities.Profile, ProfileDto>
{
    private readonly ProfileRepository _profileRepository;
    private readonly UserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProfileBusiness(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, unitOfWork.ProfileRepository!, mapper)
    {
        _unitOfWork = unitOfWork;
        _profileRepository = unitOfWork.ProfileRepository!;
        _userRepository = unitOfWork.UserRepository!;
        _mapper = mapper;
    }


    public async Task<SamanSalamatResponse?> UpdateAsync(ProfileDto dto, HttpContext httpContext, CancellationToken cancellationToken = new())
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await _userRepository.GetByIdAsync(int.Parse(userId), cancellationToken);

        if (user == null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "No logged in user found!"
            };
        }

        user.Profile = _mapper.Map(dto, user.Profile);

        await _profileRepository.UpdateAsync(user.Profile!, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new SamanSalamatResponse()
        {
            IsSuccess = true,
            Message = $"Profile successfully updated. New profile: Bio: {dto.Bio}, Email: {dto.Email}"
        };

    }
}