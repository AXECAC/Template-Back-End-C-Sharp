using Microsoft.AspNetCore.Mvc;

namespace Middlewares
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        // Обработчик исключений
        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // Создаем Id для логирования ошибки
                var traceId = Guid.NewGuid();

                // Задаем статус код для ответа
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // Создаем описание ошибки
                var problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Internal Server Error",
                    Status = (int)StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path,
                    Detail = $"Internal server error occured: TraceId : {traceId}; Message : {ex.Message}",
                };
                // Возвращаем ответ вместе с ошибкой
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
