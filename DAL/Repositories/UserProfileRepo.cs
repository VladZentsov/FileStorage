using DAL.Entities;
using DAL.FileDBContext;
using DAL.Interfaces;
using DAL.RelatedDataLoader;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserProfileRepo : IUserProfileRepo
    {
        private readonly IStorageDbContext _dbContext;
        private readonly IRelatedDataLoader _relatedDataLoader;
        private readonly DbSet<UserProfile> _userProfiles;
        private readonly DbSet<SysIdentityUser> _sysIdentityUsers;
        //private readonly DbSet<ProgramLanguageEvaluation> _programLanguageEvaluation;
        //private readonly IRelatedDataLoader _relatedDataLoader;
        public UserProfileRepo(IStorageDbContext dbContext, IRelatedDataLoader relatedDataLoader)
        {
            _dbContext = dbContext;
            _relatedDataLoader = relatedDataLoader;
            _userProfiles = dbContext.Set<UserProfile>();
            //_programLanguageEvaluation = dbContext.Set<ProgramLanguageEvaluation>();
            //_levelDiscriptions = dbContext.Set<LevelDiscription>();
            //_testResults = dbContext.Set<TestResult>();
        }
        public Task AddAsync(UserProfile entity)
        {
            _userProfiles.Add(entity);
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(UserProfile entity)
        {
            var result = entity != null;
            if (result)
            {
                _userProfiles.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public async Task DeleteByIdAsync(string id)
        {
            Delete(await GetByIdAsync(id));
        }

        public IEnumerable<UserProfile> FindAll()
        {
            return _userProfiles;
        }

        public async Task<UserProfile> GetByIdAsync(string id)
        {
            return await _userProfiles.FirstOrDefaultAsync(u => u.Id == id);
        }

        public void Update(UserProfile entity)
        {
            _userProfiles.Update(entity);
            _dbContext.SaveChanges();
        }

        public async Task<UserProfile> GetByIdWithDetailsAsync(string id)
        {
            return await _relatedDataLoader.GetUserProfileById(id);

        }

        public IEnumerable<UserProfile> FindAllWithDetails()
        {
            return _relatedDataLoader.GetAllUserProfiles();
        }
    }
}
