using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        IEnumerable<TModel> GetAll();

        Task<TModel> GetByIdAsync(string id);

        Task AddAsync(TModel model);

        Task UpdateAsync(TModel model);

        Task DeleteByIdAsync(string modelId);
    }
}
