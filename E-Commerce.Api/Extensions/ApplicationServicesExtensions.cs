using E_Commerce.Api.Errors;
using E_Commerce.Api.Helpers;
using E_Commerce.BLL.Interfaces;
using E_Commerce.BLL.Repositories;
using E_Commerce.DAL;
using E_Commerce.DAL.Entities.Identity;
using E_Commerce.DAL.Identity;
using E_Commerce.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace E_Commerce.Api.Extensions
{
    public static class ApplicationServicesExtensions
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration Configuration)
        {

            Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });

            // Add Redis Connection
            Services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(options);
            });



            Services.AddAutoMapper(typeof(MappingProfiles));

            // Register the generic repository for dependency injection
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            Services.AddScoped(typeof(IProductService), typeof(ProductService));
            //Services.AddScoped<E_Commerce.BLL.Interfaces.IOrderService, E_Commerce.ServicezOrderService>();


            // Configure custom behavior for invalid model state responses
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    // Extract error messages from the model state
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();

                    // Create an error response object
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    // Return a bad request response with the error details
                    return new BadRequestObjectResult(errorResponse);
                };


            });

            Services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

            return Services;


        }
        public static async Task ApplyMigrationsAndSeedData(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    // Apply any pending migrations and seed the database
                    var context = services.GetRequiredService<StoreContext>();
                    await context.Database.MigrateAsync();

                    await StoreContextSeed.InvokeSeed(context, loggerFactory);

                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    await identityContext.Database.MigrateAsync();

                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
                }
                catch (Exception ex)
                {
                    // Log any errors that occur during migration
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred during migration");
                }
            }
        }


    }
}
