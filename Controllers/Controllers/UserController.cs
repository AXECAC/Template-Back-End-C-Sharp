using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers.UserController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // UserController class controller
    public class UserController
    {
        private readonly IUserServices _UserServices; 
        
        public UserController(IUserServices userServices)
        {
            _UserServices = userServices;
        }
        
        // Some Get method
        [HttpGet]
        public async Task<IResult> GetUsers()
        {
            var response = await _UserServices.GetUsers();
                   
            // Some Users find
            if (response.StatusCode == 200)
            {
                return Results.Ok(response.Data.ToList());
            }
            // 0 Users find
            if (response.StatusCode == 204)
            {
                // Return response 200
                return Results.Ok();
            }
                   
            return Results.StatusCode(statusCode: response.StatusCode);
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
