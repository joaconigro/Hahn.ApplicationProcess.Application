using FastExpressionCompiler;
using Hahn.Data.Database;
using Hahn.Data.Repositories;
using Hahn.Web.Log;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hahn.Web.Extensions
{
    public static class StartupExtensions
    {

        public static void ConfigureMapster()
        {
            TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileFast();

        }

        /// <summary>
        /// Configure service to use InMemory Server
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureInMemoryDatabase(this IServiceCollection services)
        {
            //Add ApplicationDbContext
            services.AddDbContext<DatabaseContext>(options =>
                options.UseInMemoryDatabase("AssetsDb"));
            services.AddScoped<DatabaseContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }


        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }


        /// <summary>
        /// Create the Db if it doesn't exist and applies any pending migration.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public static void SeedDbContext(this IApplicationBuilder app)
        {
            // Create a service scope to get an ApplicationDbContext instance using DI
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<DatabaseContext>();

            // Seed the Db.
            DbSeeder.Seed(dbContext);
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILogManager, LogManager>();
        }
    }
}
