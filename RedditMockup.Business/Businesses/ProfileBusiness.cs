//using System.Security.Claims;
//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using RedditMockup.Business.Base;
//using RedditMockup.Common.Dtos;
//using RedditMockup.DataAccess.Contracts;
//using RedditMockup.DataAccess.Repositories;

//namespace RedditMockup.Business.Businesses;

//public class ProfileBusiness : BaseBusiness<Model.Entities.Profile, ProfileDto>
//{
//    private readonly ProfileRepository _profileRepository;
//    private readonly UserRepository _userRepository;
//    private readonly UserBusiness _userBusiness;
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IMapper _mapper;

//    public ProfileBusiness(IUnitOfWork unitOfWork, IMapper mapper, UserBusiness userBusiness) : base(unitOfWork, unitOfWork.ProfileRepository!, mapper)
//    {
//        _unitOfWork = unitOfWork;
//        _profileRepository = unitOfWork.ProfileRepository!;
//        _userRepository = unitOfWork.UserRepository!;
//        _userBusiness = userBusiness;
//        _mapper = mapper;
//    }

//}