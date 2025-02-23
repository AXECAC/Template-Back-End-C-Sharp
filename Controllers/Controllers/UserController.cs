using Microsoft.AspNetCore.Mvc;
using Services;
using DataBase;
using Extentions;

namespace Controllers.UserController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
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
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Save(User userModel, string oldEmail, bool IsNew)
        {
            // User not Valid (Bad input)
            // OldEmail Valid and IsNew == true (Bad input). OldEmail can't be valid because is new User
            // OldEmail not Valid and IsNew == false (Bad input). OldEmail must be valid because is old User
            if (!userModel.IsValid() || !(oldEmail.IsValidEmail() ^ IsNew))
            {
                // Return StatusCode 422
                return UnprocessableEntity();
            }

            // If Email == OldEmail => only this user use this email
            if (userModel.Email != oldEmail)
            {
                // Check usage "new email"
                var response = await _UserServices.GetUserByEmail(userModel.Email);

                // Conflict: this email already used
                if (response.StatusCode == DataBase.StatusCodes.Ok)
                {
                    return Conflict();
                }
            }

            // User valid and new (need create)
            if (IsNew)
            {
                await _UserServices.CreateUser(userModel);
                // Return response 201
                return Created();
            }
            // User valid and old (need edit)
            else
            {
                await _UserServices.Edit(oldEmail, userModel);
                // Return response 201
                return Created();
            }
        }

        // Delete method
        [HttpDelete]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
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
