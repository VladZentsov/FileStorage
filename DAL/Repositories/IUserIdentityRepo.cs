using DAL.Entities;
using DAL.FileDBContext;
using DAL.Interfaces;
using DAL.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserIdentityRepo : IUserIdentityRepo
    {
        private readonly UserManager<SysIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IStorageDbContext _dbContext;

        public UserIdentityRepo(IStorageDbContext dbContext, UserManager<SysIdentityUser> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task AddAsync(SysIdentityUser entity, string password, string role)
        {
            var userNameExists = await _userManager.FindByNameAsync(entity.UserName);
            if (userNameExists != null)
                throw new UserAlreadyExistsException("User with such name is already exists");

            var userEmailExists = await _userManager.FindByEmailAsync(entity.Email);
            if (userEmailExists != null)
                throw new UserAlreadyExistsException("User with such email is already exists");

            var result = await _userManager.CreateAsync(entity, password);
            if (!result.Succeeded)
                throw new NotSucceededOperationException("Creation user error");

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(entity, role);

        }

        public async Task<IdentityResult> Delete(SysIdentityUser entity)
        {
            var result = entity != null;
            if (result)
            {
                return await _userManager.DeleteAsync(entity);
                _dbContext.SaveChanges();
            }
            return IdentityResult.Failed();
        }

        public async Task DeleteByIdAsync(string id)
        {
            await Delete(await _userManager.FindByIdAsync(id));
        }

        public async Task<IEnumerable<SysIdentityUser>> FindAll()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IEnumerable<SysIdentityUser>> FindAllByRole(string role)
        {
            List<SysIdentityUser> result = new List<SysIdentityUser>();

            var allUsers = await FindAll();

            foreach (var user in allUsers)
            {
                var t = await _userManager.GetRolesAsync(user);
                if ((await _userManager.GetRolesAsync(user)).Contains(role))
                    result.Add(user);
            }

            return result;
        }

        public async Task<SysIdentityUser> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<SysIdentityUser> GetByLoginInfo(string Username, string Password)
        {
            var user = await GetByUserName(Username);

            if (await _userManager.CheckPasswordAsync(user, Password))
            {
                return user;
            }


            throw new UserNotFoundException("Incorrect password");
        }

        public async Task<SysIdentityUser> GetByUserName(string Username)
        {
            var user = await _userManager.FindByNameAsync(Username);

            if (user == null)
                throw new UserNotFoundException("Incorrect username");

            return user;
        }

        public async Task<string> GetRoleByUserId(string id)
        {
            var user = await GetByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.FirstOrDefault();

        }

        public async Task<IdentityResult> Update(SysIdentityUser entity)
        {
            return await _userManager.UpdateAsync(entity);
            _dbContext.SaveChanges();
        }
    }
}
