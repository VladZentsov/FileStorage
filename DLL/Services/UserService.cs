using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UoW;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(UserProfileDto model)
        {
            await _unitOfWork.UserProfileRepo.AddAsync(_mapper.Map<UserProfileDto, UserProfile>(model));
        }

        public async Task<UserProfile> ChangeProfile(ChangeProfileModel model, string userName)
        {
            var userProfile = await GetUserProfileByUserName(userName);

            if (model.Name != null && model.Name != "")
                userProfile.Name = model.Name;

            if (model.Surname != null && model.Surname != "")
                userProfile.Surname = model.Surname;

            if (model.Description != null && model.Description != "")
                userProfile.Description = model.Description;

            _unitOfWork.UserProfileRepo.Update(userProfile);

            return userProfile;
        }

        public async Task DeleteByIdAsync(string modelId)
        {
            await _unitOfWork.UserIdentityRepo.DeleteByIdAsync(modelId);
            await _unitOfWork.UserProfileRepo.DeleteByIdAsync(modelId);
        }

        public IEnumerable<UserProfileDto> GetAll()
        {
            return _mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserProfileDto>>(_unitOfWork.UserProfileRepo.FindAll());
        }

        public async Task<UserProfileDto> GetByIdAsync(string id)
        {
            return _mapper.Map<UserProfile, UserProfileDto>(await _unitOfWork.UserProfileRepo.GetByIdAsync(id));
        }

        public async Task<UserGeneralInfo> GetUserGeneralInfoByUserName(string userName)
        {
            var sysIdentityUser = await _unitOfWork.UserIdentityRepo.GetByUserName(userName);
            var userProfile = await _unitOfWork.UserProfileRepo.GetByIdAsync(sysIdentityUser.ProfileId);

            var result = _mapper.Map<(UserProfile, SysIdentityUser), UserGeneralInfo>((userProfile, sysIdentityUser));
            result.Role = await _unitOfWork.UserIdentityRepo.GetRoleByUserId(sysIdentityUser.Id);

            return result;
        }


        public async Task<IEnumerable<UserGeneralInfo>> GetUsersByRole(string role)
        {
            var allUsersIdentities = await _unitOfWork.UserIdentityRepo.FindAllByRole(role);

            var result = new List<UserGeneralInfo>();

            foreach (var userIdentity in allUsersIdentities)
            {
                var userProfile = await _unitOfWork.UserProfileRepo.GetByIdAsync(userIdentity.ProfileId);

                var User = _mapper.Map<(UserProfile, SysIdentityUser), UserGeneralInfo>((userProfile, userIdentity));
                User.Role = await _unitOfWork.UserIdentityRepo.GetRoleByUserId(userIdentity.Id);

                result.Add(User);
            }

            return result;
        }

        public async Task<IEnumerable<UserBriefInfo>> GetAllUsersBriefInfo(string role)
        {
            var allUsers = await GetUsersByRole(role);
            return _mapper.Map<IEnumerable<UserGeneralInfo>, IEnumerable<UserBriefInfo>>(allUsers);
        }


        private async Task<UserProfile> GetUserProfileByUserName(string userName)
        {
            var sysIdentityUser = await _unitOfWork.UserIdentityRepo.GetByUserName(userName);
            var userProfile = await _unitOfWork.UserProfileRepo.GetByIdAsync(sysIdentityUser.ProfileId);

            return userProfile;
        }
        public Task UpdateAsync(UserProfileDto model)
        {
            _unitOfWork.UserProfileRepo.Update(_mapper.Map<UserProfileDto, UserProfile>(model));
            return Task.CompletedTask;
        }
    }
}
