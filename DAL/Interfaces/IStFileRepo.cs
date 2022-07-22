using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IStFileRepo : IRepository<StFile>
    {
        public IEnumerable<StFile> GetAllFilesByStorage(string storageId);
    }
}
