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
    // UserController класс контроллер
    public class UserController : Controller
    {
        private readonly IUserServices _UserServices;

        public UserController(IUserServices userServices)
        {
            _UserServices = userServices;
        }

        // GetUsers метод
        [HttpGet]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _UserServices.GetUsers();

            // Найдены некоторые Users
            if (response.StatusCode == DataBase.StatusCodes.Ok)
            {
                // Вернуть response 200
                return Ok(response.Data.ToList());
            }
            // 0 Users найдено
            // Вернуть response 204
            return NoContent();
        }

        // GetUserById метод
        [HttpGet]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetUserById(int id)
        {
            // Id валидация (Плохой ввод)
            if (id < 1)
            {
                // Вернуть StatusCode 422
                return UnprocessableEntity();
            }
            var response = await _UserServices.GetUser(id);

            // User found
            if (response.StatusCode == DataBase.StatusCodes.Ok)
            {
                // Вернуть response 200
                return Ok(response.Data);
            }

            // User не найден
            // Вернуть response 404
            return NotFound();
        }

        // GetUserByEmail метод
        [HttpGet]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            // Email not Valid (Плохой ввод)
            if (!email.IsValidEmail())
            {
                // Вернуть StatusCode 422
                return UnprocessableEntity();
            }
            var response = await _UserServices.GetUserByEmail(email);

            // User найден
            if (response.StatusCode == DataBase.StatusCodes.Ok)
            {
                // Вернуть response 200
                return Ok(response.Data);
            }
            // User не найден
            // Вернуть response 404
            return NotFound();
        }

        // Create метод
        [HttpPost]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(User userEntity)
        {
            // User not Valid (Плохой ввод)
            if (!userEntity.IsValid())
            {
                // Вернуть StatusCode 422
                return UnprocessableEntity();
            }
            // Проверить существование "new email"
            var response = await _UserServices.GetUserByEmail(userEntity.Email);
            // Conflict: Этот email уже существует
            if (response.StatusCode == DataBase.StatusCodes.Ok)
            {
                return Conflict();
            }
            // Создать User
            await _UserServices.CreateUser(userEntity);
            // Вернуть response 201
            return CreatedAtAction(nameof(userEntity), new { message = "Successed" });
        }

        // Edit метод
        [HttpPost]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status201Created)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Edit(User userEntity, string oldEmail)
        {
            // User not Valid (Плохой ввод)
            if (!userEntity.IsValid() || !oldEmail.IsValidEmail())
            {
                // Вернуть StatusCode 422
                return UnprocessableEntity();
            }

            // Проверить oldEmail существует
            var response = await _UserServices.GetUserByEmail(oldEmail);
            // NotFound: Edit user не найден
            if (response.StatusCode == DataBase.StatusCodes.NotFound)
            {
                return NotFound();
            }

            // Изменить User
            await _UserServices.Edit(oldEmail, userEntity);
            // Вернуть response 201
            return CreatedAtAction(nameof(userEntity), new { message = "Successed" });
        }

        // Delete метод
        [HttpDelete]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound)]
        [ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Id validation (Плохой ввод)
            if (id < 1)
            {
                // Вернуть StatusCode 422
                return UnprocessableEntity();
            }
            var response = await _UserServices.DeleteUser(id);

            // User удален
            if (response.StatusCode == DataBase.StatusCodes.NoContent)
            {
                // Вернуть response 204
                return NoContent();
            }
            // User не найден
            // Вернуть response 404
            return NotFound();
        }
    }
}
