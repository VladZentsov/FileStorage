using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
//using System.Net.Http.Headers;

namespace FileStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : Controller
    {

        private readonly IUserService _userService;
        private readonly IUploadFileService _uploadFileService;
        private readonly JsonSerializerSettings _jsonSettings;

        public UploadController(IUserService userService, IUploadFileService uploadFileService)
        {
            _userService = userService;
            _uploadFileService = uploadFileService;
            _jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        [HttpGet]
        [Route("GetUserStorages")]
        public async Task<ActionResult<UserStorageDto>> GetUserStorages()
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

            var result = await _uploadFileService.GetUserStoragesAsync(userName);
            return Content(JsonConvert.SerializeObject(result));
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost("{userName}"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAsync(string userName)
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();

                await _uploadFileService.UploadFile(file, "Main",  userName, true);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
