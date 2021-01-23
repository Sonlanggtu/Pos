using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pos.WebApplication.Data;
using HealthChecks.UI.Client;
using static Pos.WebApplication.Common.WebApplicationCommon;
using LoginService;
using CustomerService;
using System;

namespace Pos.WebApplication
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
            var urlCustomerWebApi = GetEnvByKey("Pos.Customer.WebApi.Url");
            services.AddGrpc();
            services.AddGrpcClient<LoginServicce.LoginServicceClient>(opts =>
            {
                opts.Address = new Uri(urlGatewaySecure);
            });
            services.AddGrpcClient<CustomerServicce.CustomerServicceClient>(opts =>
            {
                opts.Address = new Uri(urlCustomerWebApi);
            });
            // end GRPC

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.InitBootsraper(Configuration)
                .SetHealtCheck(Configuration);

            services.AddHealthChecksUI();


            //services.AddHealthChecks()
            //.AddDbContextCheck<PollDbContext>() //nuget: Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
            //.AddApplicationInsightsPublisher(); //nuget: AspNetCore.HealthChecks.Publisher.ApplicationInsights

            services.AddHealthChecksUI() //nuget: AspNetCore.HealthChecks.UI
                .AddInMemoryStorage(); //nuget: AspNetCore.HealthChecks.UI.InMemory.Storage



            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGrpcService<Services.CustomerService>();

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                //});

                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                 name: "areas",
                 pattern: "{area:exists}/{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Auth}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

            // HealthCheck middleware
            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(delegate (Options options)
            {
                options.UIPath = "/hc-ui";
            });

        }
    }
}
