using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.RelatedDataLoader
{
    public interface IRelatedDataLoader
    {
        public Task<FileAccessibility> GetFileAccessibilityById(string id);
        public IEnumerable<FileAccessibility> GetAllFileAccessibilities();
        public  Task<UserStorage> GetUserStorageById(string id);
        public IEnumerable<UserStorage> GetAllUserStorages();
        public  Task<StorageAccessibility> GetStorageAccessibilityById(string id);
        public IEnumerable<StorageAccessibility> GetAllStorageAccessibilities();
        public  Task<UserProfile> GetUserProfileById(string id);
        public IEnumerable<UserProfile> GetAllUserProfiles();


    }
}
