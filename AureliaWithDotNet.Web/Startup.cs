using AureliaWithDotNet.Data;
using AureliaWithDotNet.Data.Repository;
using AureliaWithDotNet.Data.UnitOfWork;
using AureliaWithDotNet.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace AureliaWithDotNet.Web
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
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<AureliaWithDotNetDbContext>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<AureliaWithDotNetDbContext>(options => options.UseInMemoryDatabase("AureliaWithDotNet"));

            //services.AddRazorPages();
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AureliaWithDotNet Asset API V1"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();

            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

			app.UseDefaultFiles();

			app.UseStaticFiles(new StaticFileOptions
			{
				ServeUnknownFileTypes = true				
			});

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "AureliaWithDotNet Asset API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
