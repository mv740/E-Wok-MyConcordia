using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyConcordiaID.Models;
using MyConcordiaID.Data;
using OracleEntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyConcordiaID.Models.Student;

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
            //Add framework services.
           services.AddDbContext<DatabaseContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();



            services.AddOpenIddict<DatabaseContext>()
                .AddMvcBinders()
                .EnableAuthorizationEndpoint("/connect/authorize")
                .EnableLogoutEndpoint("/connect/logout")
                .EnableTokenEndpoint("/connect/token")
                .EnableUserinfoEndpoint("/Account/Userinfo")

                // Note: the Mvc.Client sample only uses the code flow and the password flow, but you
                // can enable the other flows if you need to support implicit or client credentials.
                .AllowAuthorizationCodeFlow()
                .AllowPasswordFlow()
                .AllowRefreshTokenFlow()

                // Make the "client_id" parameter mandatory when sending a token request.
                .RequireClientIdentification()

                // During development, you can disable the HTTPS requirement.
                .DisableHttpsRequirement()

                // Register a new ephemeral key, that is discarded when the application
                // shuts down. Tokens signed using this key are automatically invalidated.
                // This method should only be used during development.
                .AddEphemeralSigningKey()
                .UseJsonWebTokens();


            services.AddMvc();
            services.AddScoped(_ => new DatabaseEntities());
            services.AddSingleton<IStudentRepository, StudentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            // Add a middleware used to validate access
            // tokens and protect the API endpoints.
            app.UseOAuthValidation();

            app.UseIdentity();



            app.UseOAuthAuthentication(new OAuthOptions
            {
                AuthenticationScheme = "Google-AccessToken",
                DisplayName = "Google-AccessToken",
                ClientId = Configuration["google:clientid"],
                ClientSecret = Configuration["google:clientsecret"],
                CallbackPath = new PathString("/signin-google-token"),
                AuthorizationEndpoint = GoogleDefaults.AuthorizationEndpoint,
                TokenEndpoint = GoogleDefaults.TokenEndpoint,
                Scope = { "openid", "profile", "email" },
                SaveTokens = true,

                //Events = new OAuthEvents
                //{
                //    // The OnCreatingTicket event is called after the user has been authenticated and the OAuth middleware has
                //    // created an auth ticket. We need to manually call the UserInformationEndpoint to retrieve the user's information,
                //    // parse the resulting JSON to extract the relevant information, and add the correct claims.
                //    OnCreatingTicket = async context =>
                //    {
                //    // Retrieve user info
                //    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                //        request.Headers.Add("x-li-format", "json"); // Tell LinkedIn we want the result in JSON, otherwise it will return XML

                //    var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                //        response.EnsureSuccessStatusCode();

                //    // Extract the user info object
                //    var user = JObject.Parse(await response.Content.ReadAsStringAsync());



                //    }
                //}


            });

            //app.UseJwtBearerAuthentication(new JwtBearerOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    Authority = "https://accounts.google.com",
            //    Audience = Configuration["Google:ClientId"],

            //    TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateAudience = true,
            //        ValidIssuer = "accounts.google.com"
            //    },
            //    RequireHttpsMetadata = false


            //});

                // external authentication middleware 
                app.UseGoogleAuthentication(new GoogleOptions
            {
                AuthenticationScheme = "Google",
                ClientId = Configuration["Google:ClientId"],
                ClientSecret = Configuration["Google:ClientSecret"],
                Scope = { "email", "profile" },
                SaveTokens = true,
              
            });

            app.UseOAuthValidation(); // enabled auth through bearer tokens
            app.UseOpenIddict();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            
        }
    }
}
