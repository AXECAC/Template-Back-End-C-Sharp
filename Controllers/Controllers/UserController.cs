using Microsoft.AspNetCore.Mvc;
using Services;
using DataBase;
using Extentions;

namespace Controllers.UserController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // UserController class controller
    public class UserController : Controller
    {
        private readonly IUserServices _UserServices; 
        
        public UserController(IUserServices userServices)
        {
            _UserServices = userServices;
        }
        
        // GetUsers method
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _UserServices.GetUsers();

            // Some Users found
            if (response.StatusCode == DataBase.StatusCodes.Ok)
            {
                // Return response 200
                return Ok(response.Data.ToList());
            }
            // 0 Users found
            if (response.StatusCode == DataBase.StatusCodes.NoContent)
            {
                // Return response 200
                return NoContent();
            }
            // Return StatusCode 500
            return StatusCode(statusCode: 500);
        }

        // GetUserById method
        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _UserServices.GetUser(id);

            // User found
            if (response.StatusCode == DataBase.StatusCodes.Ok)
            {
                // Return response 200
                return Ok(response.Data);
            }
            // User not found
            if (response.StatusCode == DataBase.StatusCodes.NotFound)
            {
                // Return response 404
                return NotFound();
            }
            // Return StatusCode 500
            return StatusCode(statusCode: 500);
        }

        // GetUserByEmail method
        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var response = await _UserServices.GetUserByEmail(email);

            // User found
            if (response.StatusCode == DataBase.StatusCodes.Ok)
            {
                // Return response 200
                return Ok(response.Data);
            }
            // User not found
            if (response.StatusCode == DataBase.StatusCodes.NotFound)
            {
                // Return response 404
                return NotFound();
            }
            // Return StatusCode 500
            return StatusCode(statusCode: 500);
        }
        
        // Save (Create/Edit) method
        [HttpPost]
        public async Task<IActionResult> Save(User userModel)
        {
            // User not Valid (Bad input)
            if (!userModel.IsValid())
            {
                // Return StatusCode 422
                return UnprocessableEntity();
            }
            // User valid and new (need create)
            if (userModel.Id == 0)
            {
                await _UserServices.CreateUser(userModel);
                // Return response 201
                return Created();
            }
            // User valid and old (need edit)
            else
            {
                await _UserServices.Edit(userModel.Id, userModel);
                // Return response 204
                return NoContent();
            }
        }

        // Save (Create/Edit) method
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _UserServices.DeleteUser(id);

            // User Deleted
            if (response.StatusCode == DataBase.StatusCodes.NoContent)
            {
                // Return response 204
                return NoContent();
            }
            // User not found
            if (response.StatusCode == DataBase.StatusCodes.NotFound)
            {
                // Return response 404
                return NotFound();
            }
            // Return StatusCode 500
            return StatusCode(statusCode: 500);
        }
    }
}
