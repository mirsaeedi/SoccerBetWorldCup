using Microsoft.ApplicationInsights.SnapshotCollector;
using Microsoft.Extensions.Options;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.ApplicationInsights.Extensibility;
using Hangfire;
using Hangfire.Dashboard;
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
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });

            // Configure SnapshotCollector from application settings
            services.Configure<SnapshotCollectorConfiguration>(Configuration.GetSection(nameof(SnapshotCollectorConfiguration)));

            // Add SnapshotCollector telemetry processor.
            services.AddSingleton<ITelemetryProcessorFactory>(sp => new SnapshotCollectorTelemetryProcessorFactory(sp));

            var jwtConfig = Configuration.GetSection("jwt").Get<JwtOptionConfiguration>();
            services.Configure<JwtOptionConfiguration>(Configuration.GetSection("jwt"));

            services.AddDbContextPool<SoccerBetDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SoccerBetConnection")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<SoccerBetDbContext>()
                .AddDefaultTokenProviders();

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
                     ValidateIssuer = true,
                     ValidIssuer = jwtConfig.ValidIssuer,
                     ValidateAudience = true,
                     ValidAudience = jwtConfig.ValidAudience,
                     RequireSignedTokens = true,
                     RequireExpirationTime = true,
                     IssuerSigningKey = jwtConfig.IssuerSigningKey
                 };
             })
             .AddGoogle(googleOptions =>
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
             })
             .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.Scope.Add("public_profile");
                facebookOptions.Fields.Add("picture");
                facebookOptions.Fields.Add("name");
                facebookOptions.Fields.Add("email");
            })
            .AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
            });

            services.AddHttpClient("LiveScoreAPIClient", client =>
            {
                client.BaseAddress = new Uri("http://api.football-data.org");
                client.DefaultRequestHeaders.Add("X-Auth-Token", "6da0bb2be99345d1ace032c5f1b2d244");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnectionString")));
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
                app.UseHangfireServer();
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new MyAuthorizationFilter() }
                });
                RecurringJob.AddOrUpdate<LiveScoreUpdaterJob>((job) => job.Run(), Cron.Minutely);
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
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });

            
        }
    }
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return httpContext.User.Identity.IsAuthenticated;
        }
    }

    public class SnapshotCollectorTelemetryProcessorFactory : ITelemetryProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SnapshotCollectorTelemetryProcessorFactory(IServiceProvider serviceProvider) =>
            _serviceProvider = serviceProvider;

        public ITelemetryProcessor Create(ITelemetryProcessor next)
        {
            var snapshotConfigurationOptions = _serviceProvider.GetService<IOptions<SnapshotCollectorConfiguration>>();
            return new SnapshotCollectorTelemetryProcessor(next, configuration: snapshotConfigurationOptions.Value);
        }
    }
}
