using Microsoft.EntityFrameworkCore;
using Services;
using Context;
using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add my Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IHashingServices, HashingServices>();

// Read connection string to pgsql db
var connectionString = builder.Configuration.GetConnectionString("Postgres");

// Connect to db
builder.Services.AddDbContext<TemplateDbContext>(options =>
        options.UseNpgsql(connectionString));

// Connect to redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "10.100.0.4:6379";
    options.InstanceName = "local";
});

var app = builder.Build();

//Add Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Start}/{action=Index}/{id?}");

app.Run();
