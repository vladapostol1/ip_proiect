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
builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();

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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = async context =>
            {
                var tokenBlacklistService = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (await tokenBlacklistService.IsTokenBlacklistedAsync(token))
                {
                    context.Fail("This token has been blacklisted.");
                }
            }
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

app.MapPost("/login", async (IAuthService authService, JwtGenerator jwtGenerator, LoginContract contract) => 
{
    var user = await authService.AuthenticateUser(contract.Username, contract.Password);
    if (user != null)
    {
        var token = jwtGenerator.GenerateJwtToken(user);
        return Results.Ok(new { Token = token });
    }
    return Results.Unauthorized();
});

app.MapPost("/logout", async (HttpContext httpContext, ITokenBlacklistService tokenBlacklistService) =>
{
    var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

    if (!string.IsNullOrEmpty(token))
    {
        await tokenBlacklistService.BlacklistTokenAsync(token);
        return Results.Ok("Logged out successfully.");
    }

    return Results.BadRequest("No token provided.");
});


app.Run();
