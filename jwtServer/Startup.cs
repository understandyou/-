using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace jwtServer
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
            services.AddAuthentication("OAuth").AddJwtBearer("OAuth", config =>
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Sceret));
                //�����Ҫ�Զ����ȡaccess_token�ط�����Ҫ����
                config.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>//��ȡ������Ϣ���Ӷ���ȡ�Զ���token��λ��
                    {
                        if (context.Request.Query.ContainsKey("access_token"))
                        {
                            //��token�ŵ�������
                            context.Token = context.Request.Query["access_token"];
                        }

                        return Task.CompletedTask;
                    }
                };



                config.TokenValidationParameters = new TokenValidationParameters()//���Ĭ�ϴӱ�ͷ�еĻ�ȡaccess_token
                {
                    ValidIssuer = Constants.Issuer,//������
                    ValidAudience = Constants.Audiance,
                    IssuerSigningKey = key
                };
            });

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo() { Title = "demoApi", Version = "v1" });
                //����·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "jwtServer.xml");
                option.IncludeXmlComments(xmlPath);//���xmlע��
            });

            //services.AddControllers();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoAPI V1");
                //c.RoutePrefix = string.Empty;//����Ĭ����תswagger-ui
            });

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
