using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyConcordiaID.Data;
using MyConcordiaID.Helper;
using MyConcordiaID.Models;
using MyConcordiaID.Models.Admin;
using MyConcordiaID.Models.Student;
using MyConcordiaID.Providers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OracleEntityFramework;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;
using MyConcordiaID.Models.Log;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using MyConcordiaID.Models.Graduation;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using MyConcordiaID.Swagger;

namespace MyConcordiaID
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddApplicationInsightsTelemetry(Configuration);

            ////Add framework services.
            //services.AddDbContext<DatabaseContext>(options =>
            //   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<DatabaseContext>()
            //    .AddDefaultTokenProviders();

            services.AddEntityFramework()
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase();
                });

  

            /// set default authentication middleware // it is required for api oauth2
            services.AddAuthentication(options =>
            {
                options.SignInScheme = "ServerCookie";

            });

            services.AddCors(o => o.AddPolicy("AllowPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddMvc();

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowPolicy"));
            });

            services.AddScoped(_ => new DatabaseEntities());
            services.AddSingleton<IStudentRepository, StudentRepository>();
            services.AddSingleton<IAdminRepository, AdminRepository>();
            services.AddSingleton<ILogRepository, LogRepository>();
            services.AddSingleton<IGraduationRepository, GraduationRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MyConcordiaID API", Version = "v1" });
               

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "MyConcordiaID.xml");
                c.IncludeXmlComments(filePath);

                c.OperationFilter<FileOperation>();
                c.DescribeAllEnumsAsStrings();
                

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // global policy
            app.UseCors("AllowPolicy");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //remove when oauth is functional 
            app.UseDeveloperExceptionPage();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Add Application Insights monitoring to the request pipeline as a very first middleware.
            app.UseApplicationInsightsRequestTelemetry();
            app.UseStaticFiles();

           // app.UseIdentity();


            app.UseWhen(context => context.Request.Path.StartsWithSegments(new PathString("/api")), branch =>
            {
                branch.UseOAuthValidation(new OAuthValidationOptions
                {
                    AutomaticAuthenticate = true,
                    AutomaticChallenge = true
                });


            });


            //api documentation
            app.UseSwagger();
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyConcordiaID V1");
               
            });
            // Create a new branch where the registered middleware will be executed only for non API calls.
            app.UseWhen(context => !context.Request.Path.StartsWithSegments(new PathString("/api")), branch =>
            {
                // Insert a new cookies middleware in the pipeline to store
                // the user identity returned by the external identity provider.
                branch.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AutomaticAuthenticate = true,
                    AutomaticChallenge = true,
                    AuthenticationScheme = "ServerCookie",
                    CookieName = CookieAuthenticationDefaults.CookiePrefix + "ServerCookie",
                    ExpireTimeSpan = TimeSpan.FromMinutes(5),
                    LoginPath = new PathString("/signin"),
                    LogoutPath = new PathString("/signout")
                });

                branch.UseGoogleAuthentication(new GoogleOptions
                {

                    ClientId = Configuration["Google:ClientId"],
                    ClientSecret = Configuration["Google:ClientSecret"],
                    Scope = { "email", "profile" },


                });

            });


            app.UseOpenIdConnectServer(options =>
            {
                options.Provider = new AuthorizationProvider();

                // Enable the authorization and logout endpoints.
                options.AuthorizationEndpointPath = "/connect/authorize";
                options.LogoutEndpointPath = "/connect/logout";

                options.ApplicationCanDisplayErrors = true;
                options.AllowInsecureHttp = true;

                // Note: to override the default access token format and use JWT, assign AccessTokenHandler:
                // options.AccessTokenHandler = new JwtSecurityTokenHandler();
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var context = new ApplicationContext(
               app.ApplicationServices.GetRequiredService<DbContextOptions<ApplicationContext>>()))
            {
                // Note: when using the introspection middleware, your resource server
                // MUST be registered as an OAuth2 client and have valid credentials.
                // 
                // database.Applications.Add(new Application {
                //     ApplicationID = "resource_server",
                //     DisplayName = "Main resource server",
                //     Secret = "875sqd4s5d748z78z7ds1ff8zz8814ff88ed8ea4z4zzd"
                // });


                context.Applications.Add(new Application
                {
                    ApplicationID = "oidcWebClient",
                    DisplayName = "My client application",
                    RedirectUri = "https://concordiaidclient.netlify.com/WebApp/app/callback.html",
                    LogoutRedirectUri = "https://concordiaidclient.netlify.com/WebApp/app/oidc"
                    // Secret = "secret_secret_secret"
                });

                context.Applications.Add(new Application
                {
                    ApplicationID = "oidcdemomobile",
                    DisplayName = "My client application",
                    RedirectUri = "https://localhost/oidc",
                    LogoutRedirectUri = "https://localhost/oidc",
                    // Secret = "secret_secret_secret"
                });

                context.SaveChanges();
            }

        }
    }
}
