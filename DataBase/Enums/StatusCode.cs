namespace DataBase;

enum StatusCode{
    Ok, // 200 --- Ok
    Created, // 201 --- Ok created
    NoContent, // 204 --- Ok empty response
    Unauthorized, // 401 --- Unauthorized
    Forbidden, // 403 --- Authorized but permision denide
    NotFound, // 404 --- NotFound
    UnprocessableContent, // 422 --- Semantic trouble request
    TooManyRequests, // 429 --- User send too many requests for time (for the future)
    InternalServerError, // 500 --- Server error
}
