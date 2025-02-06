using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using DataBase;

namespace Controllers.StartController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // StartController class controller
    public class StartController
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
        public IActionResult SomePost(User test)
        {
            System.Console.WriteLine(test.Email);
            // Return response 200
            return new OkResult();
        }

        // Some Post method
        [HttpPost]
        public IActionResult SomeLogin(LoginUser test)
        {
            System.Console.WriteLine(test.Email);
            // Return response 200
            return new OkResult();
        }
    }
}
