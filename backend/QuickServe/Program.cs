using QuickServe.Interfaces;
using QuickServe.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/connectdb", (IDatabaseService dbService) => dbService.ConnectToDatabase());


app.Run();
