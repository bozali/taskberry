﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TaskBerry.Service.Configuration;

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
            services.Configure<TokenConfiguration>(this.Configuration.GetSection("TokenConfiguration"));

            TokenConfiguration tokenConfiguration = this.Configuration.GetSection("TokenConfiguration").Get<TokenConfiguration>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfiguration.Secret)),
                        ValidIssuer = tokenConfiguration.Issuer,
                        ValidAudience = tokenConfiguration.Audience,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            // Dependency injection configuration
            services
                .AddDbContext<TaskBerryDbContext>(options => options.UseMySql(this.Configuration.GetConnectionString("TaskBerry")))
                .AddScoped<ITaskBerryUnitOfWork, TaskBerryUnitOfWork>();

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
                .UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
                .UseMvc()
                .UseSwagger()
                .UseAuthentication();

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
