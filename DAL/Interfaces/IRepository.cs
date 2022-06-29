using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> FindAll();

        Task<TEntity> GetByIdAsync(string id);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
        Task<TEntity> GetByIdWithDetailsAsync(string id);

        Task DeleteByIdAsync(string id);
    }
}
