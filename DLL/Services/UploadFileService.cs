using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UoW;
using Microsoft.AspNetCore.Http;
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


                StFile stFile = new StFile()
                {
                    Id = Guid.NewGuid().ToString(),
                    IsPublic = IsPublic,
                    UserId = userProfile.Id,
                    UserStorageId = userStorage.Id
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
                var files = _mapper.Map<IEnumerable<StFile>, IEnumerable<FileDto>>(_unitOfWork.StFileRepo.GetAllFilesByStorage(storage.Id));
                result.Add(new UserStorageDto() { 
                    Files = files, 
                    Name = storage.Name
                });
            }

            return result;
            

        }
    }
}
