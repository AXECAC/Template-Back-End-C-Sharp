using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using DotNetEnv;
using Services;
using Services.Caching;
using DataBase;
using Middlewares;
using Context;

var builder = WebApplication.CreateBuilder(args);

// Добавить сервисы в контейнер
builder.Services.AddControllersWithViews();

//Добавить Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
        {
            // Добавить JWT в Swagger
            // Добавить 'SecurityScheme' чтобы использовать JWT Аутентификацию
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        { jwtSecurityScheme, Array.Empty<string>() }
                    });
        });

// Cors для фронта на web
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader();
        });
});

DotNetEnv.Env.Load();

var envVars = new Dictionary<string, string?>
{
    ["Postgres:Host"] = Environment.GetEnvironmentVariable("POSTGRES_HOST"),
    ["Postgres:Port"] = Environment.GetEnvironmentVariable("POSTGRES_PORT"),
    ["Postgres:Database"] = Environment.GetEnvironmentVariable("POSTGRES_DB"),
    ["Postgres:Username"] = Environment.GetEnvironmentVariable("POSTGRES_USER"),
    ["Postgres:Password"] = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"),
    ["Redis:Host"] = Environment.GetEnvironmentVariable("REDIS_HOST"),
    ["Redis:Port"] = Environment.GetEnvironmentVariable("REDIS_PORT"),
    ["Secret"] = Environment.GetEnvironmentVariable("SECRET_KEY")
};

builder.Configuration.AddInMemoryCollection(envVars!);

var redisHost = builder.Configuration["Redis:Host"];
var redisPort = builder.Configuration["Redis:Port"];
var redisConnectionString = $"{redisHost}:{redisPort}";

// Подключение к redis
builder.AddRedisClient(redisConnectionString);

// Прочитать connection string к postgres
var connectionString = $"Host={builder.Configuration["Postgres:Host"]};" +
                       $"Port={builder.Configuration["Postgres:Port"]};" +
                       $"Database={builder.Configuration["Postgres:Database"]};" +
                       $"Username={builder.Configuration["Postgres:Username"]};" +
                       $"Password={builder.Configuration["Postgres:Password"]}";

// var connectionString = builder.Configuration.GetConnectionString("Postgres");

// Подключиться к БД
builder.Services.AddDbContext<TemplateDbContext>(options =>
        options.UseNpgsql(connectionString), ServiceLifetime.Singleton);

// Добавить Аутентификацию
builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourdomain.com",
                    ValidAudience = "yourdomain.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secret"]))
                };
            });

// Добавить наши Middlewares
builder.Services.AddSingleton<ExceptionHandlerMiddleware>();

// Добавить наши Logging
builder.Services.AddLogging(builder => builder.AddConsole());
builder.Logging.AddConsole();

// Добавить наши Services
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserServices, UserServices>();
builder.Services.AddSingleton<ITokenServices, TokenServices>();
builder.Services.AddSingleton<IHashingServices, HashingServices>();
builder.Services.AddSingleton<ICachingServices<User>, CachingServices<User>>();
builder.Services.AddSingleton<IAuthServices, AuthServices>();


var app = builder.Build();

//Добавить Swagger
app.UseSwagger();
app.UseSwaggerUI();
// Если вам не нужен debug, добавьте UseSwagger... в этот if
// if (app.Environment.IsDevelopment())
// {
// }

app.UseMiddleware<ExceptionHandlerMiddleware>();
// Cors
app.UseCors();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Start}/{action=Index}/{id?}");

app.Run();
