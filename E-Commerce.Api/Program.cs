//using E_Commerce.Api.Errors;
//using E_Commerce.Api.Helpers;
//using E_Commerce.Api.Middleware;
//using E_Commerce.BLL.Interfaces;
//using E_Commerce.BLL.Repositories;
//using E_Commerce.DAL;
//using E_Commerce.DAL.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Migrations;
//using Microsoft.Extensions.Configuration;
//using StackExchange.Redis;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//using System.Security.Principal;
//using E_Commerce.DAL.Entities.Identity;
//using Microsoft.AspNetCore.Identity;

//var builder = WebApplication.CreateBuilder(args);

//// Configure the database context with SQL Server
//builder.Services.AddDbContext<StoreContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
//});


////Add Redis Connection
//builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
//{
//    var options = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
//    return ConnectionMultiplexer.Connect(options);
//});

//// Add services to the container
//builder.Services.AddControllers();

//// Configure AutoMapper with mapping profiles
//builder.Services.AddAutoMapper(typeof(MappingProfiles));

//// Register the generic repository for dependency injection
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//// Configure custom behavior for invalid model state responses
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.InvalidModelStateResponseFactory = actionContext =>
//    {
//        // Extract error messages from the model state
//        var errors = actionContext.ModelState
//            .Where(e => e.Value.Errors.Count > 0)
//            .SelectMany(x => x.Value.Errors)
//            .Select(x => x.ErrorMessage)
//            .ToArray();

//        // Create an error response object
//        var errorResponse = new ApiValidationErrorResponse
//        {
//            Errors = errors
//        };

//        // Return a bad request response with the error details
//        return new BadRequestObjectResult(errorResponse);
//    };
//});

//// Add APPuser DbContext
////PM > add - Migration IdentityInitial -c AppIdentityDbContext -o Identity / Migrations
////PM > Update - Database -Context AppIdentityDbContext
//builder.Services.AddDbContext<AppIdentityDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

//});


//builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
//{
//    //options.Password.RequireUppercase = true;
//}).AddEntityFrameworkStores<AppIdentityDbContext>();

//builder.Services.AddAuthentication();
//// Configure Swagger/OpenAPI for API documentation
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Create a scope to obtain services
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

//    try
//    {
//        // Apply any pending migrations and seed the database
//        var context = services.GetRequiredService<StoreContext>();
//        await context.Database.Migrate();

//        await StoreContextSeed.InvokeSeed(context, loggerFactory);


//        var IdentityContext = services.GetRequiredService<AppIdentityDbContext>();

//        await IdentityContext.Database.Migrate();
//        var userManager = services.GetRequiredService<UserManager<AppUser>>();
//        await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
//    }
//    catch (Exception ex)
//    {
//        // Log any errors that occur during migration
//        var logger = loggerFactory.CreateLogger<Program>();
//        logger.LogError(ex, "An error occurred during migration");
//    }
//}




//// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseMiddleware<ExceptionMiddleware>();
//    // Enable Swagger in development environment
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Enable HTTPS redirection
//app.UseHttpsRedirection();

//// Serve static files
//app.UseStaticFiles();

//// Enable authorization
//app.UseAuthorization();

//// Map controller routes
//app.MapControllers();

//// Run the application
//app.Run();


using E_Commerce.Api.Errors;
using E_Commerce.Api.Helpers;
using E_Commerce.Api.Middleware;
using E_Commerce.BLL.Interfaces;
using E_Commerce.BLL.Repositories;
using E_Commerce.DAL;
using E_Commerce.DAL.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using E_Commerce.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using E_Commerce.Service;
using E_Commerce.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

//// Configure the database context with SQL Server

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

////// Configure AutoMapper with mapping profiles


////// Add AppUser DbContext


//builder.Services.AddAuthentication();

// Configure Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create a scope to obtain services and apply migrations/seed data
await ApplicationServicesExtensions.ApplyMigrationsAndSeedData(app);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionMiddleware>();
    // Enable Swagger in development environment
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Serve static files
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Run the application
app.Run();


