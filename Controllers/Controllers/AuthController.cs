/* using Microsoft.AspNetCore.Authentication; */
/* using Microsoft.AspNetCore.Identity; */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataBase;

namespace Controllers.AuthController
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    // StartController class controller
    public class AuthController : Controller
    {
        // Some Post method
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User test)
        {
            return  Ok();
        }
    }
}
