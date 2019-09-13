namespace TaskBerry.Service
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.EntityFrameworkCore;

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

            //services.AddAuthentication().AddJwtBearer(options =>
            //{
            //    // TODO Add options
            //});

            // Dependency injection configuration
            //services
            //    .AddDbContext<TaskBerryDbContext>(options => options.UseMySql(this.Configuration.GetConnectionString("TaskBerry")));

            // TODO Use automapper c#

            // Add swagger documentation generation
#if DEBUG
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("taskberry", new Info { Title = "TaskBerry", Version = "v1" });
                options.EnableAnnotations();

                string xmlFullPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.IncludeXmlComments(xmlFullPath);
            });
#endif
        }

        /// <summary>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseMvc()
                .UseSwagger();
#if DEBUG
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/taskberry/swagger.json", "TaskBerry"));
#endif

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
