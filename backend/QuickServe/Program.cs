using QuickServe.Interfaces;
using QuickServe.Services;
using QuickServe.Models;
using QuickServe.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/connectdb", (IDatabaseService dbService) => dbService.ConnectToDatabase());

app.MapPost("/register", async (IAuthService authService, RegistrationContract contract) => 
{
    bool result = await authService.RegisterUser(contract);
    return result ? Results.Ok("User registered successfully") : Results.BadRequest("Registration failed");
});

app.MapPost("/login", async (IAuthService authService, LoginContract contract) => 
{
    UserModel user = await authService.AuthenticateUser(contract.Username, contract.Password);
    return user != null ? Results.Ok(user) : Results.Unauthorized();
});


app.Run();
