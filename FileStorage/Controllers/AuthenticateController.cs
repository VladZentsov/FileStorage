using BLL.Interfaces;
using BLL.Models;
using DAL.Validation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace FileStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly IUserService _userService;

        public AuthenticateController(IAuthenticateService authenticateService, IUserService userService)
        {
            _authenticateService = authenticateService;
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel bookModel)
        {
            bookModel.Role = "User";
            await _authenticateService.RegisterAsync(bookModel);
            return Content(JsonConvert.SerializeObject(bookModel));
        }


        [HttpPost]
        [Route("registerAdmin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterModel bookModel)
        {
            bookModel.Role = "Admin";
            await _authenticateService.RegisterAsync(bookModel);
            return Content(JsonConvert.SerializeObject(bookModel));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model.Username == null || model.Password == null)
                return BadRequest("Username and Password needed");

            JwtSecurityToken token;
            try
            {
                token = await _authenticateService.Login(model);
            }
            catch (UserNotFoundException ex)
            {
                return Unauthorized(ex.Message);
            }

            var user = await _userService.GetUserGeneralInfoByUserName(model.Username);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                user = user
            });
        }
    }
}
