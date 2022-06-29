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
    public class AuthenticateService: IAuthenticateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AuthenticateService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> Login(LoginModel model)
        {
            var user = await _unitOfWork.UserIdentityRepo.GetByLoginInfo(model.Username, model.Password);
            var userRole = await _unitOfWork.UserIdentityRepo.GetRoleByUserId(user.Id);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var token = GetToken(authClaims);

            return token;

        }

        public async Task RegisterAsync(RegisterModel model)
        {
            if (model.Id == null)
                model.Id = Guid.NewGuid().ToString();

            string identityId = Guid.NewGuid().ToString();
            SysIdentityUser user = new SysIdentityUser()
            {
                Id = identityId,
                Email = model.Email,
                ProfileId = model.Id,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            await _unitOfWork.UserIdentityRepo.AddAsync(user, model.Password, model.Role);

            var ProfileModel = _mapper.Map<RegisterModel, UserProfile>(model);
            ProfileModel.SysIdentityId = identityId;
            ProfileModel.CreatedDate = DateTime.Now;

            await _unitOfWork.UserProfileRepo.AddAsync(ProfileModel);
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
