using System;
using System.Reflection;
using Dws.Challenge.Application.Services;
using Dws.Challenge.Application.Services.Interfaces;
using Dws.Challenge.Infrastructure.Caching;
using Dws.Challenge.Infrastructure.Caching.Interfaces;
using Dws.Challenge.Infrastructure.Gateways;
using Dws.Challenge.Infrastructure.Gateways.Interfaces;
using Dws.Challenge.Infrastructure.Logging;
using Dws.Challenge.Infrastructure.Logging.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Dws.Challenge.WebApi
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
            services.AddControllers();
            services.AddAutoMapper(Assembly.Load("Dws.Challenge.Application"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dws.Challenge.WebApi", Version = "v1" });
            });

            services.AddTransient<IArtistService, ArtistService>();
            services.AddTransient<IArtistGateway, ArtistGateway>();
            services.AddSingleton<ICache, InMemoryCache>();
            services.AddTransient(typeof(ILoggerWrapper<>), typeof(LoggerWrapper<>));

            services.AddHttpClient("artist-http-client", httpClient =>
            {
                httpClient.BaseAddress = new Uri(this.Configuration.GetSection("ArtistApiUrl").Value);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dws.Challenge.WebApi v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}