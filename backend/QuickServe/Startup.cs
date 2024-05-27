using QuickServe.Interfaces;
using QuickServe.Services;
using QuickServe.Contracts;
using QuickServe.Utils;
using QuickServe.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace QuickServe
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddSingleton(sp => new JwtGenerator(Configuration));
            services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
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

            services.AddAuthorization();
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", () => "Hello World!");

                endpoints.MapGet("/connectdb", (IDatabaseService dbService) => dbService.ConnectToDatabase());

                endpoints.MapPost("/register", async (IAuthService authService, RegistrationContract contract) =>
                {
                    bool result = await authService.RegisterUser(contract);
                    return result ? Results.Ok("User registered successfully") : Results.BadRequest("Registration failed");
                });

                endpoints.MapPost("/login", async (IAuthService authService, JwtGenerator jwtGenerator, LoginContract contract) =>
                {
                    var user = await authService.AuthenticateUser(contract.Username, contract.Password);
                    if (user != null)
                    {
                        var token = jwtGenerator.GenerateJwtToken(user);
                        return Results.Ok(new { Token = token });
                    }
                    return Results.Unauthorized();
                });

                endpoints.MapPost("/logout", async (HttpContext httpContext, ITokenBlacklistService tokenBlacklistService) =>
                {
                    var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                    if (!string.IsNullOrEmpty(token))
                    {
                        await tokenBlacklistService.BlacklistTokenAsync(token);
                        return Results.Ok("Logged out successfully.");
                    }

                    return Results.BadRequest("No token provided.");
                });

                endpoints.MapGet("/foods", async (HttpContext httpContext, IFoodService foodService) =>
                {
                    var restaurantId = httpContext.User.GetRestaurantId();

                    if (restaurantId == 0)
                        return Results.BadRequest("Invalid token");

                    var foods = await foodService.GetFoodsAsync(restaurantId);
                    return Results.Ok(foods);
                });

                endpoints.MapGet("/foods/{id}", async (HttpContext httpContext, IFoodService foodService, int id) =>
                {
                    var restaurantId = httpContext.User.GetRestaurantId();

                    if (restaurantId == 0)
                        return Results.BadRequest("Invalid token");

                    var food = await foodService.GetFoodByIdAsync(id, restaurantId);
                    return food != null ? Results.Ok(food) : Results.NotFound();
                });

                endpoints.MapPost("/foods", async (HttpContext httpContext, IFoodService foodService, FoodModel food) =>
                {
                    var restaurantId = httpContext.User.GetRestaurantId();

                    if (restaurantId == 0)
                        return Results.BadRequest("Invalid token");

                    food.RestaurantId = restaurantId;
                    await foodService.AddFoodAsync(food);
                    return Results.Ok("Food added successfully.");
                });

                endpoints.MapDelete("/foods/{id}", async (HttpContext httpContext, IFoodService foodService, int id) =>
                {
                    var restaurantId = httpContext.User.GetRestaurantId();

                    if (restaurantId == 0)
                        return Results.BadRequest("Invalid token");

                    await foodService.DeleteFoodAsync(id, restaurantId);
                    return Results.Ok("Food deleted successfully.");
                });

                endpoints.MapPost("/orders", async (HttpContext httpContext, IOrderService orderService, OrderModel order) =>
                {
                    var restaurantId = httpContext.User.GetRestaurantId();

                    if (restaurantId == 0)
                        return Results.BadRequest("Invalid token");

                    order.RestaurantId = restaurantId;
                    var orderId = await orderService.CreateOrderAsync(order, order.Items);
                    return Results.Ok(new { OrderId = orderId });
                });

                endpoints.MapGet("/orders", async (HttpContext httpContext, IOrderService orderService) =>
                {
                    var restaurantId = httpContext.User.GetRestaurantId();

                    if (restaurantId == 0)
                        return Results.BadRequest("Invalid token");

                    var orders = await orderService.GetOrdersAsync(restaurantId);
                    return Results.Ok(orders);
                });

                endpoints.MapGet("/orders/{id}", async (HttpContext httpContext, IOrderService orderService, int id) =>
                {
                    var restaurantId = httpContext.User.GetRestaurantId();

                    if (restaurantId == 0)
                        return Results.BadRequest("Invalid token");

                    var order = await orderService.GetOrderByIdAsync(id, restaurantId);
                    return order != null ? Results.Ok(order) : Results.NotFound();
                });
            });
        }
    }
}
