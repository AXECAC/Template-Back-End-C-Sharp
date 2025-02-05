using Microsoft.EntityFrameworkCore;
using Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Read connection string to pgsql db
var connectionString = builder.Configuration.GetConnectionString("Postgres");

// Connect to db
builder.Services.AddDbContext<TemplateDbContext>(options => 
        options.UseNpgsql(connectionString));

var app = builder.Build();

//Add Swagger
if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Start}/{action=Index}/{id?}");

app.Run();
