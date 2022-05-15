using EmployeeManagement.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(x =>
            {
                //overriding password complexity
                x.Password.RequiredLength = 8;
                x.Password.RequiredUniqueChars = 3;

            }).
                AddEntityFrameworkStores<contextdb>();
            //overriding password complexity
            //services.Configure<IdentityOptions>(x =>
            //{
            //    x.Password.RequiredLength = 8;
            //    x.Password.RequiredUniqueChars = 3;

            //});
            services.AddDbContextPool<contextdb>(options => options.UseSqlServer(_config.GetConnectionString("employeedbcon")));
            services.AddMvc(options=>options.EnableEndpointRouting=false).AddXmlSerializerFormatters();
            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions()
                //{
                //    SourceCodeLineCount = 2
                //};
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/error/{0}");
            }
            ////showing own choice default page
            //DefaultFilesOptions defaultFiles = new DefaultFilesOptions();
            //defaultFiles.DefaultFileNames.Clear();
            //defaultFiles.DefaultFileNames.Add("foo.html");
            //app.UseDefaultFiles(defaultFiles);
            //app.UseStaticFiles();
            ////showing own choice default page
            //FileServerOptions fso = new FileServerOptions();
            //fso.DefaultFilesOptions.DefaultFileNames.Clear();
            //fso.DefaultFilesOptions.DefaultFileNames.Add("foo.html");
            //using both the properties of Usedefaultfile and UsestaticFiles
            //app.UseFileServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseAuthentication();
            app.UseMvc(route =>
            {
                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            //app.Run(async (context) =>
            //{

            //    await context.Response.WriteAsync("Hello world");

            //});

        }
    }
}
