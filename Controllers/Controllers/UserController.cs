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
        
        // GetUsers method
        [HttpGet]
        public async Task<IResult> GetUsers()
        {
            var response = await _UserServices.GetUsers();

            // Some Users found
            if (response.StatusCode == 200)
            {
                // Return response 200
                return Results.Ok(response.Data.ToList());
            }
            // 0 Users found
            if (response.StatusCode == 204)
            {
                // Return response 200
                return Results.Ok();
            }
            // Return StatusCode 500
            return Results.StatusCode(statusCode: response.StatusCode);
        }

        // GetUsers method
        [HttpGet]
        public async Task<IResult> GetUser(int id)
        {
            var response = await _UserServices.GetUser(id);

            // User found
            if (response.StatusCode == 200)
            {
                // Return response 200
                return Results.Ok(response.Data);
            }
            // User not found
            if (response.StatusCode == 404)
            {
                // Return response 200
                return Results.NotFound();
            }
            // Return StatusCode 500
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
