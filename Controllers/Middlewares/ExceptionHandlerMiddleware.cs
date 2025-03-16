using Microsoft.AspNetCore.Mvc;

namespace Middlewares
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        // Обработчик исключений
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

                // Цвета для выделения текста в логах
                string red = "\x1b[91m";
                string white = "\x1b[39m";
                string green = "\x1b[92m";

                // Добавляем логирование ошибки, где TraceId == Id логирования ошибки
                // Message == текст ошибки
                // StackTrace == вся остальная информация об ошибке
                _logger.LogError("Error occurred while processing the request:\n" +
                        $"{green}TraceId{white} : {traceId};\n" +
                        $"{red}Message{white} : {ex.Message};\n" +
                        $"{red}StackTrace{white} : {ex.StackTrace}");

                // Задаем статус код 500 для ответа
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
