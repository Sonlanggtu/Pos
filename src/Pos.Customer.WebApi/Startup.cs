using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dermayon.Infrastructure.EvenMessaging.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Pos.Customer.Domain.Events;
using Pos.Customer.Infrastructure;
using Pos.Customer.Infrastructure.EventSources;
using Pos.Customer.WebApi.Application.EventHandlers;

namespace Pos.Customer.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InitBootsraper(Configuration)
                .InitAppServices()
                .InitEventHandlers()
                .InitMapperProfile();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Customer",
                    Version = "1.0",
                    Description = "This API Customer",
                    Contact = new OpenApiContact
                    {
                        Name = "DamNgocSon",
                        Email = "damngocsonIT@gmail.com",
                        Url = new Uri("https://sonlanggtu.github.io/"),
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Customer");
            });

            app.UseMvc();
        }
    }
}
