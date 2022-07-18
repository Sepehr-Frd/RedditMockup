using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RedditMockup.Business.Base;
using RedditMockup.Common.Dtos;
using RedditMockup.Common.Helpers;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Business.Businesses;

public class UserBusiness : BaseBusiness<User, UserDto>
{
    private readonly UserRepository _userRepository;

    private readonly ProfileRepository _profileRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public UserBusiness(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, unitOfWork.UserRepository!, mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.UserRepository!;
        _profileRepository = unitOfWork.ProfileRepository!;
        _mapper = mapper;
    }

    public async Task<SamanSalamatResponse?> CreateAsync(UserDto dto, CancellationToken cancellationToken = default)
    {
        var isUsernameValid = !await UsernameExistsAsync(dto.Username!, cancellationToken);

        if (isUsernameValid)
        {
            dto.Password = await dto.Password!.GetHashStringAsync();

            var user = _mapper.Map<User>(dto);

            user.Profile = new()
            {
                UserId = user.Id
            };

            var userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = 2
            };

            user.UserRoles!.Add(userRole);

            return await CreateAsync(user, cancellationToken);
        }

        return new SamanSalamatResponse()
        {
            IsSuccess = false,
            Message = $"{dto.Username} is already used, try another one"
        };
    }


    private async Task<User?> LoadModelByIdAsync(int id, CancellationToken cancellationToken = new())
    {

        SieveModel sieveModel = new()
        {
            Filters = $"Id=={id}"
        };

        var users = await _userRepository.LoadAllAsync(sieveModel,
            include => include
            .Include(x => x.Person)
            .Include(x => x.Profile)
            .Include(x => x.Questions)
            .Include(x => x.Answers)
            .Include(x => x.UserRoles),
            cancellationToken);

        if (users.Count == 0)
        {
            return null;
        }

        return users.Single();

    }

    public async Task<SamanSalamatResponse?> LoadByIdAsync(int id, CancellationToken cancellationToken = new())
    {

        var user = await LoadModelByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No user exists with the ID of {id}"
            };
        }

        var response = _mapper.Map<UserDto>(user);

        return new SamanSalamatResponse()
        {
            Data = response,
            IsSuccess = true
        };

    }

    private async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = new())
    {
        SieveModel sieveModel = new()
        {
            Filters = $"Username=={username}"
        };

        var users = await _userRepository.LoadAllAsync(sieveModel, null, cancellationToken);

        return users.Count > 0;

    }

    public async Task<SamanSalamatResponse?> UpdateProfileAsync(ProfileDto dto, HttpContext httpContext, CancellationToken cancellationToken = new())
    {
        var stringUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        int userId = int.Parse(stringUserId);

        var user = await LoadModelByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "No logged in user found"
            };
        }

        _mapper.Map(dto, user.Profile);

        await _profileRepository.UpdateAsync(user.Profile!, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new SamanSalamatResponse()
        {
            Data = dto,
            IsSuccess = true
        };

    }

    public async Task<SamanSalamatResponse?> DeleteAsync(LoginDto loginDto, CancellationToken cancellationToken = new())
    {
        var isValid = await IsUsernameAndPasswordValidAsync(loginDto, cancellationToken);

        if (!isValid)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = "Username and/or password invalid"
            };
        }

        var user = await LoadByUsernameAsync(loginDto.Username!, cancellationToken);

        return await DeleteAsync(user!, cancellationToken);
    }

    private async Task<User?> LoadByUsernameAsync(string username, CancellationToken cancellationToken = new())
    {
        SieveModel sieveModel = new()
        {
            Filters = $"Username=={username}"
        };

        var users = await _userRepository.LoadAllAsync(sieveModel, null, cancellationToken);

        if (users.Count == 0)
        {
            return null;
        }

        return users.Single();
    }

    private async Task<bool> IsUsernameAndPasswordValidAsync(LoginDto login, CancellationToken cancellationToken = new())
    {
        SieveModel sieveModel = new()
        {
            Filters = $"Username=={login.Username!}, Password=={login.Password!.GetHashStringAsync()}"
        };

        var users = await _userRepository.LoadAllAsync(sieveModel, null, cancellationToken);

        return users.Count > 0;
    }


}