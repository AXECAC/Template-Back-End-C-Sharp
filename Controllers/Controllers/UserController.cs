using Microsoft.AspNetCore.Mvc;

namespace Controllers.UserController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // UserController class controller
    public class UserController
    {
        // Some Get method
        [HttpGet]
        public IActionResult SomeGet()
        {
            // Return response 200
            return new OkResult();
        }

        // Some Post method
        [HttpPost]
        public IActionResult SomePost(int some)
        {
            // Return response 200
            return new OkResult();
        }
    }
}
