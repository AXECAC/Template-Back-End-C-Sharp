using Microsoft.AspNetCore.Mvc;

namespace TemplateBackEndCSharp.StartController{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // StartController class controller
    public class StartController{
        // Some Get method
        [HttpGet]
        public IActionResult SomeGet(){
            // Return response 200
            return new OkResult();
        }

        // Some Post method
        [HttpPost]
        public IActionResult SomePost(int some){
            // Return response 200
            return new OkResult();
        }
    }
}
