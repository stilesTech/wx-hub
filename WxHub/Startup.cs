using System;
using System.IO;
using System.Linq;
using Common;
using Common.SqlUtils;
using Entities;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using OperateCenter.Extensions;
using UEditor.Core;

namespace OperateCenter
{
    public class Startup
    {
        public static ILoggerRepository repository { get; set; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;

            this.Configuration = new ConfigurationBuilder()
            .SetBasePath($"{Directory.GetCurrentDirectory()}/Config")
            .AddJsonFile("connection.json", true, true)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true)
            .AddEnvironmentVariables()
            .Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConnectionStringWrapper settings = new ConnectionStringWrapper();
            Configuration.Bind("ConnectionStringWrapperSettings", settings);
            SqlFactory.SetDefault(settings);


            // auth
            services.Configure<AuthOptions>(this.Configuration.GetSection("operateCenter:auth"));
            AuthOptions authOptions = this.Configuration.GetSection("operateCenter:auth").Get<AuthOptions>();

            //依赖注入
            services.AddSingleton<ConnectionStringWrapper>(settings);
            services.AddSingleton<AuthManager>(new AuthManager(authOptions));

            services.AddMemoryCache();
            services.AddUEditorService();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            repository = LogManager.CreateRepository(LogHelper.RepositoryName);
            XmlConfigurator.Configure(repository, new FileInfo("Config/log4net.config"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }


            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images")),
                RequestPath = "/images",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                },
            });
            string uploadFile = Path.Combine(Directory.GetCurrentDirectory(), "upload");
            if (!Directory.Exists(uploadFile))
            {
                Directory.CreateDirectory(uploadFile);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory(), "upload")),
                RequestPath = "/upload",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                },
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
