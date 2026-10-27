using Microsoft.AspNetCore.Mvc;
using TrainingManagementApi.Exceptions;

namespace TrainingManagementApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Not found: {Message}", ex.Message);
                await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
            }
            catch (BusinessValidationException ex)
            {
                _logger.LogWarning("Validation failed: {Message}", ex.Message);
                await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while processing {Method} {Path}",
                    context.Request.Method, context.Request.Path);
                await WriteProblemAsync(context, StatusCodes.Status500InternalServerError,
                    "Internal Server Error", "An unexpected error occurred. Please try again later.");
            }
        }

        private static async Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
        {
            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(problem);
        }


    }
}
