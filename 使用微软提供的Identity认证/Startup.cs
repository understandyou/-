using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using 使用微软提供的Identity认证.authRequirement;

namespace 使用微软提供的Identity认证
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
            //不用identity了
            //services.AddIdentity<IdentityUser,IdentityRole>()
            //认证服务
            services.AddAuthentication("").AddCookie();
            //授权角色为admin的人通过授权
            services.AddAuthorization(config => config.AddPolicy("admin", policy => policy.RequireClaim(ClaimTypes.Role,"admin")));
            //授权服务
            services.AddAuthorization(config =>
            {
                config.AddPolicy("claim", policy =>
                {
                    //policy.RequireClaim(ClaimTypes.DateOfBirth);
                    //添加必须的信息
                    policy.AddRequirements(new CustomRequireClaim("传入的内容视具体情况而言")); //CustomRequireClaim自定义需要的信息
                });
            });
            //注册自定义的认证服务
            services.AddScoped<IAuthorizationHandler, CustomRequireHandler>();
            services.AddControllersWithViews(config =>
            {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                    .RequireAuthenticatedUser()
                    .Build();

                // global authorization filter
                config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
