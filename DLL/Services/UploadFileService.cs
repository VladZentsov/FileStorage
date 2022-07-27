using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UoW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http.Headers;


namespace BLL.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UploadFileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task UploadFile(IFormFile file, string storageName, string userName, bool IsPublic)
        {
            var sysIdentityUser = await _unitOfWork.UserIdentityRepo.GetByUserName(userName);
            var userProfile = await _unitOfWork.UserProfileRepo.GetByIdAsync(sysIdentityUser.ProfileId);
            string folderName = "";


            string dir = "Resources\\" + sysIdentityUser.UserName+"\\"+storageName;

            UserStorage userStorage = new UserStorage();

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);

                userStorage = new UserStorage()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserProfileId = userProfile.Id,
                    Name = storageName
                };

                await _unitOfWork.UserStorageRepo.AddAsync(userStorage);
            }
            else
            {
                userStorage = await _unitOfWork.UserStorageRepo.FindByProfileIdAndName(sysIdentityUser.ProfileId, storageName);
            }

            folderName = Path.Combine("Resources", sysIdentityUser.UserName, storageName);

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                var Weight = file.Length;

                var FileWeight = SizeSuffix(Weight);


                StFile stFile = new StFile()
                {
                    Id = Guid.NewGuid().ToString(),
                    IsPublic = IsPublic,
                    UserProfileId = userProfile.Id,
                    UserStorageId = userStorage.Id,
                    Name = fileName,
                    Weight = FileWeight
                };

                await _unitOfWork.StFileRepo.AddAsync(stFile);

                var fullPath = Path.Combine(pathToSave, fileName);
                //var dbPath = Path.Combine(folderName, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

            }
            else
            {
                return;
            }
        }
        public Task AddAsync(FileDto model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(string modelId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<FileDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FileDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserStorageDto>> GetUserStoragesAsync(string userName)
        {
            var sysIdentityUser = await _unitOfWork.UserIdentityRepo.GetByUserName(userName);
            var userProfile = await _unitOfWork.UserProfileRepo.GetByIdAsync(sysIdentityUser.ProfileId);

            var userStorages = _unitOfWork.UserStorageRepo.FindAll().Where(st => st.UserProfileId == userProfile.Id);

            var result = new List<UserStorageDto>();

            foreach (var storage in userStorages)
            {
                var all = _unitOfWork.StFileRepo.FindAll();
                var f = _unitOfWork.StFileRepo.GetAllFilesByStorage(storage.Id);
                var files = _mapper.Map<IEnumerable<StFile>, IEnumerable<FileDto>>(_unitOfWork.StFileRepo.GetAllFilesByStorage(storage.Id));
                result.Add(new UserStorageDto() { 
                    Files = files, 
                    Name = storage.Name
                });
            }

            return result;
            

        }

        static string SizeSuffix(long value, int decimalPlaces = 1)
        {
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }

        public async Task<(MemoryStream, string, string)> Download(string userName, string storage, string fileName)
        {
            string fileUrl = "Resources\\" + userName + "\\" + storage + "\\" + fileName;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileUrl);

            string r = GetContentType(filePath);

            if (!System.IO.File.Exists(filePath))
                throw new DirectoryNotFoundException();
            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return (memory, GetContentType(filePath), filePath);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }

    }
}
