using BLL.Models;
using DAL.Entities;
using DAL.UoW;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService : ICrud<UserProfileDto>
    {
        public Task<UserGeneralInfo> GetUserGeneralInfoByUserName(string userName);
        public Task<UserProfile> ChangeProfile(ChangeProfileModel model, string userName);
        public Task<IEnumerable<UserBriefInfo>> GetAllUsersBriefInfo(string role);
    }
}
