/* using Microsoft.AspNetCore.Authentication; */
/* using Microsoft.AspNetCore.Identity; */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using DataBase;

namespace Controllers.AuthController
{
    // [Authorize]
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

        // Some Post method
        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (_secretKey != "")
            {
                var response = await _AuthServices.TryRegister(user, _secretKey);
                if (response.StatusCode == DataBase.StatusCodes.Created)
                {
                    Console.WriteLine(response.Data);
                    return Ok(new {response.Data});
                }
                else
                {
                    return Conflict();
                }
            }
            // Return StatusCode 500
            return StatusCode(statusCode: 500);
            
        }
    }
}
