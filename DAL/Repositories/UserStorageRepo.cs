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
    public class UserStorageRepo : IUserStorageRepo
    {
        private readonly IStorageDbContext _dbContext;
        private readonly IRelatedDataLoader _relatedDataLoader;
        private readonly DbSet<UserStorage> _userStorage;
        private readonly DbSet<StFile> _stFiles;
        public UserStorageRepo(IStorageDbContext dbContext, IRelatedDataLoader relatedDataLoader)
        {
            _dbContext = dbContext;
            _userStorage = dbContext.Set<UserStorage>();
            _stFiles = dbContext.Set<StFile>();
            _relatedDataLoader = relatedDataLoader;

        }
        public Task AddAsync(UserStorage entity)
        {
            _userStorage.Add(entity);
            _dbContext.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task AddFileAsync(StFile file, string storageId)
        {
            var storage = await GetByIdAsync(storageId);

            if (storage.Files == null)
            {
                storage.Files = new List<StFile>() { file };
            }
           else if (!storage.Files.Contains(file))
            {
                storage.Files.Add(file);
            }
            else
            {
                return;
            }
            
            Update(storage);           

        }

        public void Delete(UserStorage entity)
        {
            var result = entity != null;
            if (result)
            {
                _userStorage.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public async Task DeleteByIdAsync(string id)
        {
            Delete(await GetByIdAsync(id));
        }

        public async Task<UserStorage> FindByProfileIdAndName(string profileId, string name)
        {
            return await _userStorage.FirstOrDefaultAsync(st=>st.Name == name&&st.UserProfileId == profileId);
        }

        public IEnumerable<UserStorage> FindAll()
        {
            return _userStorage;
        }

        public async Task<UserStorage> GetByIdAsync(string id)
        {
            return await _userStorage.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserStorage> GetByIdWithDetailsAsync(string id)
        {
            var userProfile = await _userStorage.Where(u => u.Id == id).Include(up => up.Files).FirstOrDefaultAsync();

            _stFiles.Load();

            return userProfile;
        }

        public void Update(UserStorage entity)
        {
            _userStorage.Update(entity);
            _dbContext.SaveChanges();
        }

    }
}
