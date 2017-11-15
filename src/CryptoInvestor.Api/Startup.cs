using Autofac;
using Autofac.Extensions.DependencyInjection;
using CryptoInvestor.Api.Framework;
using CryptoInvestor.Infrastructure.IoC;
using CryptoInvestor.Infrastructure.Mongo;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using CryptoInvestor.Infrastructure.Settings;
using Hangfire;
using Hangfire.Mongo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

namespace CryptoInvestor.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var issuer = Configuration["Jwt:Issuer"];

            services.AddMvc();
            services.AddCors();
            services.AddMemoryCache();
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "CryptoInvestor API", Version = "v1" });
            });

            JobStorage.Current = new MongoStorage(Configuration["Mongo:ConnectionString"], Configuration["Mongo:Database"],
                    new MongoStorageOptions { Prefix = "Hg" });
            services.AddHangfire(c => c.UseStorage(JobStorage.Current));

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new ContainerModule(Configuration));
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddNLog();
            app.AddNLogWeb();
            env.ConfigureNLog("nlog.config");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoInvestor API V1");
            });

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
            );
            app.UseAuthentication();
            app.UseExceptionsHandler();
            app.UseMvc();

            MongoConfigurator.Initialize();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            var generalSettings = app.ApplicationServices.GetService<GeneralSettings>();
            if (generalSettings.SeedData)
            {
                var dataInitializer = app.ApplicationServices.GetService<IDataInitializer>();
                dataInitializer.SeedAsync();
            }
            var coinsProvider = app.ApplicationServices.GetService<ICoinsProvider>();
            RecurringJob.AddOrUpdate(() => coinsProvider.Provide(), Cron.MinuteInterval(15));

            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}