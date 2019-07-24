using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using MCP_WEB.Service;
using MCP_WEB.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;


namespace MCP_WEB
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<NittanDBcontext>(options =>
       options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.HttpOnly = true;
            });

            services.AddDistributedMemoryCache();


            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        
                        options.LoginPath = new PathString("/Login/Logout");
                        options.LogoutPath = new PathString("/Login/Logout");
                        options.AccessDeniedPath = new PathString("/Login/Logout");
                        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                        options.Cookie.Expiration = TimeSpan.FromMinutes(30);
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                        options.Events = new CookieAuthenticationEvents
                        {
                            OnValidatePrincipal = LastChangedValidator.ValidateAsync
                        };

                    });

            services.AddCors(options =>
            {
                options.AddPolicy("AnyOrigin", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod();
                });
            });


            services.AddTransient<MenuMasterService, MenuMasterService>();

            services.AddTransient<InterFaceDBcontext, InterFaceDBcontext>();


            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<SessionTimeoutAttribute>();

            services.AddHttpContextAccessor();

            services.AddTransient<IUserRepository, UserRepository>();

            services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");


            // Add framework services.
            services
                .AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            IFileProvider physicalProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

            services.AddSingleton<IFileProvider>(physicalProvider);

            services.AddTransient<IEmailService, EmailService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Uploads")),
            //    RequestPath = new PathString("/Uploads")
            //});


            app.UseCookiePolicy();

            app.UseCors("AnyOrigin");

            app.UseAuthentication();

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
            });

        }
    }
}
