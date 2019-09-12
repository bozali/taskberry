namespace TaskBerry.Service
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using TaskBerry.DataAccess.Domain;

    using Swashbuckle.AspNetCore.Swagger;


    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            // Add MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
             
            // Dependency injection configuration
            services.AddScoped<ITaskBerryDbContext, TaskBerryDbContext>(provider => new TaskBerryDbContext(this.Configuration.GetConnectionString("TaskBerry")));

            // Add swagger documentation generation
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("taskberry", new Info { Title = "TaskBerry" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/taskberry/swagger.json", "TaskBerry");
                options.RoutePrefix = "docs";
            });

            app.UseMvc();
        }

        public IConfiguration Configuration { get; }
    }
}
