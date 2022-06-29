using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FileStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly JsonSerializerSettings _jsonSettings;

        public UserController(IUserService userService)
        {
            _userService = userService;
            _jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetById(string id)
        {
            return Content(JsonConvert.SerializeObject(await _userService.GetByIdAsync(id), _jsonSettings));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _userService.DeleteByIdAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<UserGeneralInfo>> GetMyProfile()
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            return Content(JsonConvert.SerializeObject(await _userService.GetUserGeneralInfoByUserName(userName), _jsonSettings));
        }

        [HttpPost]
        [Route("changeProfile")]
        public async Task<ActionResult> ChangeProfile([FromBody] ChangeProfileModel model)
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            var result = await _userService.ChangeProfile(model, userName);
            return Content(JsonConvert.SerializeObject(result, _jsonSettings));
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult<UserBriefInfo>> GetAllUsers()
        {
            var result = await _userService.GetAllUsersBriefInfo("User");
            return Content(JsonConvert.SerializeObject(result, _jsonSettings));
        }


        [HttpGet]
        [Route("GetAllAdmins")]
        public async Task<ActionResult<UserBriefInfo>> GetAllAdmins()
        {
            var result = await _userService.GetAllUsersBriefInfo("Admin");
            return Content(JsonConvert.SerializeObject(result, _jsonSettings));
        }

        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<ActionResult<UserGeneralInfo>> GetUserProfile(string userName)
        {
            return Content(JsonConvert.SerializeObject(await _userService.GetUserGeneralInfoByUserName(userName), _jsonSettings));
        }


    }
}
