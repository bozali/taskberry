using Microsoft.EntityFrameworkCore;

namespace TaskBerry.Service
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using System.Reflection;
    using System.IO;
    using System;

    using TaskBerry.DataAccess.Domain;

    using Swashbuckle.AspNetCore.Swagger;


    /// <summary>
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Dependency injection configuration
            services
                .AddDbContext<TaskBerryDbContext>(options => options.UseMySql(this.Configuration.GetConnectionString("TaskBerry")));


            // Add swagger documentation generation
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("taskberry", new Info { Title = "TaskBerry", Version = "v1" });
                options.EnableAnnotations();

                string xmlFullPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.IncludeXmlComments(xmlFullPath);
            });
        }

        /// <summary>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/taskberry/swagger.json", "TaskBerry"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }

        /// <summary>
        /// </summary>
        public IConfiguration Configuration { get; }
    }
}
