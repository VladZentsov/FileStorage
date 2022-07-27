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
    public class StFileRepo : IStFileRepo
    {
        private readonly IStorageDbContext _dbContext;
        private readonly IRelatedDataLoader _relatedDataLoader;
        private readonly DbSet<UserStorage> _userStorage;
        private readonly DbSet<StFile> _stFiles;
        public StFileRepo(IStorageDbContext dbContext, IRelatedDataLoader relatedDataLoader)
        {
            _dbContext = dbContext;
            _userStorage = dbContext.Set<UserStorage>();
            _stFiles = dbContext.Set<StFile>();
            _relatedDataLoader = relatedDataLoader;

        }
        public Task AddAsync(StFile entity)
        {
            _stFiles.Add(entity);
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(StFile entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StFile> FindAll()
        {
            return _stFiles;
        }

        public Task<StFile> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<StFile> GetByIdWithDetailsAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(StFile entity)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<StFile> GetAllFilesByStorage(string storageId)
        {
            return FindAll().Where(f=>f.UserStorageId == storageId);
        }
    }
}
