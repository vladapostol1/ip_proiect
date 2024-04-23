using QuickServe.Interfaces;
using QuickServe.Services;
using QuickServe.Contracts;
using QuickServe.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<JwtGenerator>(sp => new JwtGenerator(configuration));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();


app.UseAuthentication();

app.MapGet("/", () => "Hello World!");

app.MapGet("/connectdb", (IDatabaseService dbService) => dbService.ConnectToDatabase());

app.MapPost("/register", async (IAuthService authService, RegistrationContract contract) => 
{
    bool result = await authService.RegisterUser(contract);
    return result ? Results.Ok("User registered successfully") : Results.BadRequest("Registration failed");
});

app.MapPost("/login", async (IAuthService authService, LoginContract contract) => 
{
    var user = await authService.AuthenticateUser(contract.Username, contract.Password);
    if (user != null)
    {
        //var token = JwtGenerator.GenerateJwtToken(user);
        return Results.Ok(new { Token = token });
    }
    return Results.Unauthorized();
});


app.Run();
