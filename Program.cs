using System;
using LearnLinQWeb.Controllers;
using LearnLinQWeb.Data;
using LearnLinQWeb.Middlewares;
using LearnLinQWeb.Models;
using LearnLinQWeb.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
        .UseSnakeCaseNamingConvention());

builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Web", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

// Migration tự động khi build
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    db.Database.Migrate();
//}

app.UseMiddleware<ExceptionsMiddleware>();

app.UseMiddleware<ResponsesMiddleware>();

// Middleware hệ thống phải đặt trước Controller
// Middleware request response thì phải đặt sau middlware mà wrap toàn bộ pipline
// Midleware wrap như kiểu exception thì phải đặt ngoài cùng.

app.UseHttpsRedirection();


// Cors phải đứng trước Auth
app.UseCors("Web");

app.UseAuthorization();


// Controller
app.MapControllers();

// Endpoint phải đặt cuối cùng
app.MapGet("/api/v1/ping", () => Results.Ok("Pong"));

app.Run();
