using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserIdentityRepo
    {
        Task<IEnumerable<SysIdentityUser>> FindAll();

        Task<SysIdentityUser> GetByIdAsync(string id);

        Task AddAsync(SysIdentityUser entity, string password, string role);

        Task<IdentityResult> Update(SysIdentityUser entity);

        Task<IdentityResult> Delete(SysIdentityUser entity);

        Task DeleteByIdAsync(string id);

        Task<IEnumerable<SysIdentityUser>> FindAllByRole(string role);

        Task<SysIdentityUser> GetByLoginInfo(string Username, string Password);

        Task<string> GetRoleByUserId(string id);

        Task<SysIdentityUser> GetByUserName(string Username);

    }
}
