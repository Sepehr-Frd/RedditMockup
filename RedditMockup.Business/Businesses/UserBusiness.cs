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

    public override async Task<SamanSalamatResponse?> CreateAsync(UserDto dto, HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var isUsernameValid = !await UsernameExistsAsync(dto.Username!, cancellationToken);

        var stringUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var userId = int.Parse(stringUserId);

        if (!isUsernameValid)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"{dto.Username} is already used, try another one"
            };
        }

        dto.Password = await dto.Password!.GetHashStringAsync();

        var user = _mapper.Map<User>(dto);

        user.Profile = new() { UserId = user.Id };

        var userRole = new UserRole() { UserId = user.Id, RoleId = 2 };

        user.UserRoles!.Add(userRole);

        return await CreateAsync(user, cancellationToken);
    }


    public async Task<User?> LoadModelByIdAsync(int id, CancellationToken cancellationToken = new())
    {
        SieveModel sieveModel = new() { Filters = $"Id=={id}" };

        var users = await _userRepository.LoadAllAsync(sieveModel,
            include => include.Include(x => x.Person).Include(x => x.Profile).Include(x => x.Questions)
                .Include(x => x.Answers).Include(x => x.UserRoles), cancellationToken);

        if (users.Count == 0)
        {
            return null;
        }

        return users.Single();
    }

    public override async Task<SamanSalamatResponse?> LoadByIdAsync(int id, CancellationToken cancellationToken = new())
    {
        var user = await LoadModelByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return new SamanSalamatResponse() { IsSuccess = false, Message = $"No user exists with the ID of {id}" };
        }

        var response = _mapper.Map<UserDto>(user);

        return new SamanSalamatResponse() { Data = response, IsSuccess = true };
    }

    private async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = new())
    {
        SieveModel sieveModel = new() { Filters = $"Username=={username}" };

        var users = await _userRepository.LoadAllAsync(sieveModel, null, cancellationToken);

        return users.Count > 0;
    }

    public override async Task<SamanSalamatResponse?> UpdateAsync(int id, UserDto dto,
        CancellationToken cancellationToken = new())
    {
        var user = await LoadModelByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return new SamanSalamatResponse()
            {
                IsSuccess = false,
                Message = $"No user found with ID of {id}"
            };
        }

        _mapper.Map(dto, user);

        return await UpdateAsync(user, cancellationToken);
    }

    public override async Task<SamanSalamatResponse?> DeleteAsync(int id, CancellationToken cancellationToken = new())
    {
        var user = await LoadModelByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return new SamanSalamatResponse() { IsSuccess = false, Message = $"No user found with ID of {id}" };
        }

        return await DeleteAsync(user, cancellationToken);
    }
}