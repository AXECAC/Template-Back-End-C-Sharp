using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using DataBase;
using Extentions;

namespace Controllers.UserController
{
    [Route("api/[controller]/[action]")]
    [Authorize]
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
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetUserById(int id)
        {
            // Id validation (Bad Input)
            if (id < 1)
            {
                // Return StatusCode 422
                return UnprocessableEntity();
            }
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
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            // Email not Valid (Bad input)
            if (!email.IsValidEmail())
            {
                // Return StatusCode 422
                return UnprocessableEntity();
            }
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

        // Create method
        [HttpPost]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(User userModel)
        {
            // User not Valid (Bad input)
            if (!userModel.IsValid())
            {
                // Return StatusCode 422
                return UnprocessableEntity();
            }
            // Check usage "new email"
            var response = await _UserServices.GetUserByEmail(userModel.Email);
            // Conflict: this email already used
            if (response.StatusCode == DataBase.StatusCodes.Ok)
            {
                return Conflict();
            }
            // Create User
            await _UserServices.CreateUser(userModel);
            // Return response 201
            return CreatedAtAction(nameof(userModel), new { message = "Successed" });
        }

        // Edit method
        [HttpPost]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Edit(User userModel, string oldEmail)
        {
            // User not Valid (Bad input)
            if (!userModel.IsValid() || !oldEmail.IsValidEmail())
            {
                // Return StatusCode 422
                return UnprocessableEntity();
            }

            // Check oldEmail is real
            var response = await _UserServices.GetUserByEmail(oldEmail);
            // NotFound: Edit user not found
            if (response.StatusCode == DataBase.StatusCodes.NotFound)
            {
                return NotFound();
            }

            // Edit User
            await _UserServices.Edit(oldEmail, userModel);
            // Return response 201
            return CreatedAtAction(nameof(userModel), new { message = "Successed" });
        }

        // Delete method
        [HttpDelete]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Id validation (Bad Input)
            if (id < 1)
            {
                // Return StatusCode 422
                return UnprocessableEntity();
            }
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
