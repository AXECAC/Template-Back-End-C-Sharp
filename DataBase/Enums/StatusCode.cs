namespace DataBase;

public enum StatusCodes
{
    Ok, // 200 --- Ok
    Created, // 201 --- Ok создано
    NoContent, // 204 --- Ok пустой response
    BadRequest, // 400
    Unauthorized, // 401 --- Unauthorized
    Forbidden, // 403 --- Authorized но доступ запрещен
    NotFound, // 404 --- NotFound
    Conflict, // 409 --- Conflict (этот email уже существует)
    UnprocessableContent, // 422 --- Неправильный формат request
    TooManyRequests, // 429 --- слишком много запросов от User
    InternalServerError, // 500 --- Ошибка сервера
}
