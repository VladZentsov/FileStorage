using DAL.Entities;
using DAL.FileDBContext;
using DAL.Interfaces;
using DAL.RelatedDataLoader;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IStorageDbContext _sysDbContext;

        private IUserIdentityRepo _userIdentityRepo;
        private readonly UserManager<SysIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IUserProfileRepo _userProfileRepo;
        private IUserStorageRepo _userStorageRepo; 
        private IRelatedDataLoader _relatedDataLoader;
        private IStFileRepo _stFileRepo;

        public UnitOfWork(IStorageDbContext sysDbContext, UserManager<SysIdentityUser> userManager, RoleManager<IdentityRole> roleManager, IRelatedDataLoader relatedDataLoader)
        {
            _sysDbContext = sysDbContext;
            _userManager=userManager;
            _roleManager=roleManager;
            _relatedDataLoader = relatedDataLoader;
        }
        public IUserIdentityRepo UserIdentityRepo
        {
            get
            {
                if (_userIdentityRepo == null)
                {
                    _userIdentityRepo = new UserIdentityRepo(_sysDbContext, _userManager, _roleManager);
                }
                return _userIdentityRepo;
            }
        }

        public IUserProfileRepo UserProfileRepo
        {
            get
            {
                if (_userProfileRepo == null)
                {
                    _userProfileRepo = new UserProfileRepo(_sysDbContext, _relatedDataLoader);
                }
                return _userProfileRepo;
            }
        }
        public IUserStorageRepo UserStorageRepo
        {
            get
            {
                if (_userStorageRepo == null)
                {
                    _userStorageRepo = new UserStorageRepo(_sysDbContext, _relatedDataLoader);
                }
                return _userStorageRepo;
            }
        }

        public IStFileRepo StFileRepo
        {
            get
            {
                if (_stFileRepo == null)
                {
                    _stFileRepo = new StFileRepo(_sysDbContext, _relatedDataLoader);
                }
                return _stFileRepo;
            }
        }
    }
}
