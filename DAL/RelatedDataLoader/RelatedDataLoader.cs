using DAL.Entities;
using DAL.FileDBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.RelatedDataLoader
{
    public class RelatedDataLoader: IRelatedDataLoader
    {
        readonly IStorageDbContext _dbContext;
        public readonly DbSet<UserProfile> _userProfiles;
        public readonly DbSet<StFile> _stFiles;
        public readonly DbSet<UserStorage> _userStorages;
        public readonly DbSet<FileAccessibility> _fileAccessibilities;
        public readonly DbSet<StorageAccessibility> _storageAccessibilities;

        public RelatedDataLoader(IStorageDbContext dbContext)
        {
            _dbContext = dbContext;
            _userProfiles = dbContext.Set<UserProfile>();
            _userStorages = dbContext.Set<UserStorage>();
            _fileAccessibilities = dbContext.Set<FileAccessibility>();
            _storageAccessibilities = dbContext.Set<StorageAccessibility>();

            _stFiles = dbContext.Set<StFile>();
        }
        public async Task<FileAccessibility> GetFileAccessibilityById(string id)
        {
            var fileAccessibility = await _fileAccessibilities.FirstOrDefaultAsync(f => f.Id == id);
            _stFiles.Where(f => f.Id == fileAccessibility.Id).Load();

            return fileAccessibility;
        }

        public IEnumerable<FileAccessibility> GetAllFileAccessibilities()
        {
            var fileAccessibility = _fileAccessibilities.Include(f => f.File);
            _stFiles.Load();

            return fileAccessibility;
        }

        public async Task<UserStorage> GetUserStorageById(string id)
        {
            var userStorage = await _userStorages.FirstOrDefaultAsync(us => us.Id == id);
            _stFiles.Where(f => f.Id == userStorage.UserProfileId).Load();

            return userStorage;
        }

        public IEnumerable<UserStorage> GetAllUserStorages()
        {
            var userStorages = _userStorages.Include(us => us.Files);
            _stFiles.Load();

            return userStorages;
        }

        public async Task<StorageAccessibility> GetStorageAccessibilityById(string id)
        {
            var storageAccessibility = await _storageAccessibilities.FirstOrDefaultAsync(us => us.Id == id);
            storageAccessibility.UserStorage = await GetUserStorageById(storageAccessibility.StorageId);

            return storageAccessibility;
        }

        public IEnumerable<StorageAccessibility> GetAllStorageAccessibilities()
        {
            var storageAccessibilities = _storageAccessibilities.Include(st => st.UserStorage);

            _userStorages.Load();
            _stFiles.Load();

            return storageAccessibilities;
        }

        public async Task<UserProfile> GetUserProfileById(string id)
        {
            var userProfile = await _userProfiles.Where(u => u.Id == id).Include(up => up.SysIdentityUser).FirstOrDefaultAsync();

            _storageAccessibilities.Load();
            _fileAccessibilities.Load();
            _userStorages.Load();
            _stFiles.Load();

            return userProfile;
        }

        public IEnumerable<UserProfile> GetAllUserProfiles()
        {
            var userProfiles = _userProfiles;

            _storageAccessibilities.Load();
            _fileAccessibilities.Load();
            _userStorages.Load();
            _stFiles.Load();

            return userProfiles;
        }


    }
}
