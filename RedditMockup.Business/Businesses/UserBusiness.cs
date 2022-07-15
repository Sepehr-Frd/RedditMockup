using AutoMapper;
using RedditMockup.Business.Base;
using RedditMockup.Common.Dtos;
using RedditMockup.Common.Helpers;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;

namespace RedditMockup.Business.Businesses;

public class UserBusiness : BaseBusiness<User, UserDto>
{
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserBusiness(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, unitOfWork.UserRepository!, mapper)
    {
        _userRepository = unitOfWork.UserRepository!;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public new async Task<SamanSalamatResponse?> CreateAsync(UserDto dto, CancellationToken cancellationToken = default)
    {
        var isUsernameValid = !await UsernameExistsAsync(dto.Username!, cancellationToken);

        if (isUsernameValid)
        {
            dto.Password = await dto.Password!.GetHashStringAsync();

            var user = _mapper.Map<User>(dto);

            var userInstance = await _userRepository.CreateAsync(user, cancellationToken);

            var userRole = new UserRole()
            {
                UserId = userInstance.Id,
                RoleId = 2
            };

            userInstance.UserRoles.Add(userRole);

            await _unitOfWork.CommitAsync(cancellationToken);

            var response = _mapper.Map<UserDto>(userInstance);

            return new SamanSalamatResponse()
            {
                Data = response,
                IsSuccess = true,
                Message = "User successfully created."
            };
        }

        return new SamanSalamatResponse()
        {
            IsSuccess = false,
            Message = "Username is already used!"
        };
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = new()) =>
        await _userRepository.UsernameExistsAsync(username, cancellationToken);

}