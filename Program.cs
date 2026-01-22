using System;
using System.Text;
using LearnLinQWeb.Application.Interfaces;
using LearnLinQWeb.Application.Mappings;
using LearnLinQWeb.Controllers;
using LearnLinQWeb.Data;
using LearnLinQWeb.Data.Interfaces;
using LearnLinQWeb.Data.Interfaces.Auth;
using LearnLinQWeb.Data.Interfaces.Book;
using LearnLinQWeb.Data.Interfaces.User;
using LearnLinQWeb.Infrastructure.Persistence;
using LearnLinQWeb.Infrastructure.Security;
using LearnLinQWeb.Middlewares;
using LearnLinQWeb.Services;
using LearnLinQWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
        .UseSnakeCaseNamingConvention());


// mapping

builder.Services.AddAutoMapper(typeof(MappingProfile));


// unit of work

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// book
builder.Services.AddScoped<IBookService, BookService>();
//builder.Services.AddScoped<IBookQuery, BookEfQuery>();
//builder.Services.AddScoped<IBookCommand, BookEfCommand>();

// auth
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthQuery, AuthEfQuery>();
//builder.Services.AddScoped<IAuthCommand, AuthEfCommand>();

// user
builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IUserQuery, UserEfQuery>();
//builder.Services.AddScoped<IUserCommand, UserEfCommand>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Web", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services
    .AddAuthentication(options =>
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
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

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

//app.UseMiddleware<ResponsesMiddleware>();

// Middleware hệ thống phải đặt trước Controller
// Middleware request response thì phải đặt sau middlware mà wrap toàn bộ pipline
// Midleware wrap như kiểu exception thì phải đặt ngoài cùng.

app.UseHttpsRedirection();


// Cors phải đứng trước Auth
app.UseCors("Web");

app.UseAuthentication();
app.UseAuthorization();


// Controller
app.MapControllers();

// Endpoint phải đặt cuối cùng
app.MapGet("/api/v1/ping", () => Results.Ok("Pong"));

app.Run();
