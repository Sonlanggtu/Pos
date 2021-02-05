using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LoginService;
using System;
using static Pos.Customer.Common.CommonCustomers;
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
            // GRPC
            var urlGatewaySecure = GetEnvByKey("Pos.Gateway.Securities.Url");
            services.AddGrpc();
            services.AddGrpcClient<LoginServicce.LoginServicceClient>(opts =>
                           {
                               opts.Address = new Uri(urlGatewaySecure);
                           });

            // end GRPC
            services.InitBootsraper(Configuration)
                    .InitAppServices()
                    .InitEventHandlers()

                    .InitMapperProfile().test1234();

            services.AddControllers();


        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Services.CustomerService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello, This is Pos.Customer.WebApi");
                });

                endpoints.MapControllers();
            });
        }
    }
}
