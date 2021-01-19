using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pos.Gateway.Securities.Application;
using Pos.Gateway.Securities.Models;

namespace Pos.Gateway.Securities
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
            services.AddScoped<IAuthService, AuthService>();
            //services.AddTransient<IUserStore<AspnetUse>, UserStore>();
            //services.AddTransient<IRoleStore<AppRole>, RoleStore>();


            //services.AddIdentity<AspNetUsers, AspNetRoles>()
            //   .AddDefaultTokenProviders();

            //services.Configure<IdentityOptions>(opt =>
            //{
            //    // Default Password settings.
            //    opt.Password.RequireDigit = true;
            //    opt.Password.RequireLowercase = false;
            //    opt.Password.RequireNonAlphanumeric = false;
            //    opt.Password.RequireUppercase = false;
            //    opt.Password.RequiredLength = 6;
            //    opt.Password.RequiredUniqueChars = 1;
            //});
            //services.AddTransient<UserManager<AspNetUsers>, AspNetUserManager>();
            //services.AddTransient<SignInManager<ApplicationUser>, AspNetUserSignInManager>();
            //services.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
            //services.AddTransient<IUserStore<AspNetUsers>, ApplicationUserStore>();
            // services.AddTransient<IRoleStore<AspNetRoles>, ApplicationRoleStore>();


            services.AddTransient<UserManager<AspNetUsers>>();

            services.AddDbContext<POS_GatewaySecuritiesContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("CUSTOMER_READ_CONNECTION")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<POS_GatewaySecuritiesContext>();


            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Pos.Gateway.Securities",
                    Version = "1.0",
                    Description = "This API Pos.Gateway.Securities",
                    Contact = new OpenApiContact
                    {
                        Name = "DamNgocSon",
                        Email = "damngocsonIT@gmail.com",
                        Url = new Uri("https://sonlanggtu.github.io/"),
                    }
                });
            });

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Pos.Gateway.Securities");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
