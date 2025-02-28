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
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        // Registration method
        public async Task<IActionResult> Registration(User user)
        {
            // Secret key empty
            if (_secretKey == "")
            {
                // Return StatusCode 500
                return StatusCode(statusCode: 500);
            }
            // Try Registration
            var response = await _AuthServices.TryRegister(user, _secretKey);
            // Registration successed
            if (response.StatusCode == DataBase.StatusCodes.Created)
            {
                // Return token (201)
                return CreatedAtAction(nameof(user), new { response.Data });
            }
            // This email already used
            else
            {
                // Return Conflict (409)
                return Conflict();
            }

        }
        [HttpPost]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
        // Login method
        public async Task<IActionResult> Login(LoginUser form)
        {
            // Secret key empty
            if (_secretKey == "")
            {
                // Return StatusCode 500
                return StatusCode(statusCode: 500);
            }
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

        [HttpGet]
        [Authorize]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized)]
        // Check token (instead of login if have token old valid token)(max old 3 hour)
        public IActionResult Check()
        {
            // Token valid or else will be returned Unauthorized
            return Ok();
        }
    }
}
