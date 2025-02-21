/* using Microsoft.AspNetCore.Authentication; */
/* using Microsoft.AspNetCore.Identity; */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using DataBase;

namespace Controllers.AuthController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // StartController class controller
    public class AuthController : Controller
    {
        private readonly IAuthServices _AuthServices;
        private readonly string? _secretKey;

        public AuthController(IAuthServices authServices, IConfiguration configuration)
        {
            _AuthServices = authServices;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        // Registration method
        public async Task<IActionResult> Registration(User user)
        {
            // Secret key not empty
            if (_secretKey != "")
            {
                // Try Registration
                var response = await _AuthServices.TryRegister(user, _secretKey);
                // Registration successed
                if (response.StatusCode == DataBase.StatusCodes.Created)
                {
                    // Return token (200)
                    return Ok(new { response.Data });
                }
                // This email already used
                else
                {
                    // Return Conflict (409)
                    return Conflict();
                }
            }
            // Return StatusCode 500
            return StatusCode(statusCode: 500);

        }
        [HttpPost]
        // Login method
        public async Task<IActionResult> Login(LoginUser form)
        {
            // Secret key not empty
            if (_secretKey != "")
            {
                // Try Login
                var response = await _AuthServices.TryLogin(form, _secretKey);
                // Login successed
                if (response.StatusCode == DataBase.StatusCodes.Ok)
                {
                    // Return token (200)
                    return Ok(new { response.Data });
                }
                // Email or Password wrong
                else
                {
                    // Return Conflict (401)
                    return Unauthorized();
                }
            }
            // Return StatusCode 500
            return StatusCode(statusCode: 500);

        }
    }
}
