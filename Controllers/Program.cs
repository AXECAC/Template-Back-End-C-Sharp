using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Services;
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
// Cors для фронта на веб
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader();
        });
});

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:Secret"]))
                };
            });

// Добавить my Services
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserServices, UserServices>();
builder.Services.AddSingleton<ITokenServices, TokenServices>();
builder.Services.AddSingleton<IHashingServices, HashingServices>();
builder.Services.AddSingleton<IAuthServices, AuthServices>();

// Прочитать connection string к postgres
var connectionString = builder.Configuration.GetConnectionString("Postgres");


// Подключиться к бд
builder.Services.AddDbContext<TemplateDbContext>(options =>
        options.UseNpgsql(connectionString), ServiceLifetime.Singleton);

var app = builder.Build();

//Добавить Swagger
app.UseSwagger();
app.UseSwaggerUI();
// Если вам не нужен debug, добавьте UseSwagger... в этот if
// if (app.Environment.IsDevelopment())
// {
// }

// Cors
app.UseCors();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Start}/{action=Index}/{id?}");

app.Run();
