namespace DataBase;

public enum StatusCodes
{
    Ok, // 200 --- Ok
    Created, // 201 --- Ok created
    NoContent, // 204 --- Ok empty response
    Unauthorized, // 401 --- Unauthorized
    Forbidden, // 403 --- Authorized but permision denide
    NotFound, // 404 --- NotFound
    Conflict, // 409 --- Conflict (this email already exists)
    UnprocessableContent, // 422 --- Semantic trouble request
    TooManyRequests, // 429 --- User send too many requests for time (for the future)
    InternalServerError, // 500 --- Server error
}
