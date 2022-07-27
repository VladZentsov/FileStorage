using BLL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUploadFileService /*: ICrud<FileDto>*/
    {
        public Task UploadFile(IFormFile file, string storageName, string userName, bool IsPublic);
        public Task<List<UserStorageDto>> GetUserStoragesAsync(string userName);
        public Task<(MemoryStream, string, string)> Download(string userName, string storage, string fileName);
    }
}
