using Hahn.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hahn.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StartupExtensions.ConfigureMapster();
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Add the controllers
            services.AddControllers();

            //Add Swagger generation
            services.ConfigureSwaggerGen();

            //Configures de InMemory database
            services.ConfigureInMemoryDatabase();

            //Suppress ModelStateInvalidFilter, so can be manually validated
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //Add a logger service
            services.ConfigureLoggerService();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The IApplicationBuilder.</param>
        /// <param name="env">The IWebHostEnvironment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hahn.Web v1"));
            }

            //Add first the exception middleware to catch all the exceptions.
            app.ConfigureCustomExceptionMiddleware();

            //Add Http redirection
            app.UseHttpsRedirection();

            //Add File server to serve aurelia SPA files
            app.UseFileServer();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Seed the Db if is development environment.
            if (env.IsDevelopment())
            {
                app.SeedDbContext();
            }
        }
    }
}
