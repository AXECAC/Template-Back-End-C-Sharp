добавить enum на statusCode


operation result --- посмотреть варианты

builder.Services.AddSingleton --- посмотреть

Посмотреть

https://github.com/OfferingSolutions/Entity-Framework-Core-Generic-Repository
https://github.com/OfferingSolutions/Entity-Framework-Core-Generic-Repository/blob/master/OfferingSolutions.GenericEFCore/RepositoryBase/GenericRepositoryBase.cs
https://github.com/OfferingSolutions/Entity-Framework-Core-Generic-Repository/blob/master/OfferingSolutions.GenericEFCore.SampleApp/ExampleRepositories/PersonRepository.cs


Переделать UserRepository -> BaseRepository

```cs
return await Db.Users.FirstOrDefaultAsync(x => x.Email == email);
```
->
```cs
return await Db.Set<T>().FirstOrDefaultAsync(x => x.Email == email);
```

controller IResult -> IActionResult

```cs
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
            if (response.StatusCode == 200)
            {
                // Return response 200
                return Ok(response.Data.ToList());
            }
            // 0 Users found
            if (response.StatusCode == 204)
            {
                // Return response 200
                return Ok();
            }
            // Return StatusCode 500
            return StatusCode(statusCode: response.StatusCode);
        }
    }
```


почитать middleware для обработки ошибок (вместо try|catch) и для авторизации


Хеширование паролей через отдельный сервис

baseresponse -> статический фабричный метод 

BaseResponse<T>.Ok();
