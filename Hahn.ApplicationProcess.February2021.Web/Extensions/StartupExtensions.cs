using FastExpressionCompiler;
using FluentValidation.AspNetCore;
using Hahn.Data.Database;
using Hahn.Data.Repositories;
using Hahn.Web.Dtos;
using Hahn.Web.Log;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;

namespace Hahn.Web.Extensions
{
    /// <summary>
    /// Defines the <see cref="StartupExtensions" />.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// The Configure Mapster with FastExpressionCompiler.
        /// </summary>
        public static void ConfigureMapster()
        {
            TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileFast();
        }

        /// <summary>
        /// Configure service to use InMemory Server.
        /// </summary>
        /// <param name="services">.</param>
        public static void ConfigureInMemoryDatabase(this IServiceCollection services)
        {
            //Add ApplicationDbContext
            services.AddDbContext<DatabaseContext>(options =>
                options.UseInMemoryDatabase("AssetsDb"));
            services.AddScoped<DatabaseContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        /// <summary>
        /// Configure Swagger service.
        /// </summary>
        /// <param name="services">.</param>
        public static void ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hahn.Web",
                    Version = "v1",
                    Description = "A simple web API demo for Hahn Softwareentwicklung.",
                    Contact = new OpenApiContact
                    {
                        Name = "Joaquín Nigro",
                        Email = "joaquinnigro@gmail.com",
                        Url = new Uri("https://github.com/joaconigro"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT license."
                    }
                });
                c.ExampleFilters();
                var filePath = Path.Combine(AppContext.BaseDirectory, "Hahn.Web.xml");
                c.IncludeXmlComments(filePath);
            });
            services.AddSwaggerExamplesFromAssemblyOf<AssetDto>();
        }

        /// <summary>
        /// Configure validators.
        /// </summary>
        /// <param name="services">.</param>
        public static void ConfigureValidators(this IServiceCollection services)
        {
            services.AddMvc().AddFluentValidation();
        }

        /// <summary>
        /// Configure the custom exception middleware.
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/>.</param>
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        /// <summary>
        /// Seed the Db.
        /// </summary>
        /// <param name="app">.</param>
        public static void SeedDbContext(this IApplicationBuilder app)
        {
            // Create a service scope to get an ApplicationDbContext instance using DI
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<DatabaseContext>();

            // Seed the Db.
            DbSeeder.Seed(dbContext);
        }

        /// <summary>
        /// Configures the logger service.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILogManager, LogManager>();
        }
    }
}
