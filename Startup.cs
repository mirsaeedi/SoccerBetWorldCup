using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using SoccerBet.Data;
using SoccerBet.Data.Models;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtConfig = Configuration.GetSection("jwt").Get<JwtOptionConfiguration>();
            services.Configure<JwtOptionConfiguration>(Configuration.GetSection("jwt"));

            services.AddDbContext<SoccerBetDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SoccerBetConnection")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<SoccerBetDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/signin";
                options.LogoutPath = "/logout";
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(30);
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = true;
                 options.SaveToken = true;
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ClockSkew = TimeSpan.FromMinutes(5),
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidateIssuer=true,
                     ValidIssuer = jwtConfig.ValidIssuer,
                     ValidateAudience=true,
                     ValidAudience = jwtConfig.ValidAudience,
                     RequireSignedTokens = true,
                     RequireExpirationTime=true,
                     IssuerSigningKey = jwtConfig.IssuerSigningKey
                 };
             });

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                
                googleOptions.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        var identity = (ClaimsIdentity)context.Principal.Identity;
                        var profileImg = context.User["image"].Value<string>("url");
                        identity.AddClaim(new Claim("Image", profileImg));
                        return Task.FromResult(0);
                    }
                };
            });

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/api/account/signin/facebook");
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.Scope.Add("public_profile");
                facebookOptions.Fields.Add("picture");
                facebookOptions.Fields.Add("name");
                facebookOptions.Fields.Add("email");
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
