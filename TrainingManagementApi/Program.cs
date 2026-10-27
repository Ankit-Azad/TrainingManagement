using Microsoft.EntityFrameworkCore;
using TrainingManagementApi.Data;
using TrainingManagementApi.Middleware;
using TrainingManagementApi.Interfaces;
using TrainingManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICourseService, CourseService>();


var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();   // must be first in the pipeline

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();                                   // serves /openapi/v1.json
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Training Management API v1");
    });                                                 // Swagger page at /swagger
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();