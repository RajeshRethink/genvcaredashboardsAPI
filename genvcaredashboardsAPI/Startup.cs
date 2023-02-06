using genvcaredashboardsAPI.DBContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using genvcaredashboardsAPI.Services;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace genvcaredashboardsAPI
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

            // register the DbContext on the container, getting the connection string from appSettings
            var connectionString = Configuration["ConnectionStrings:GenVCareDBConnectionString"];

            //TODO: When moving to .Net 5.0, consider changing this to AddDbContextFactory. As of now we do get an error with async programming
            // Sometimes which says same DB context is being used already
            // https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-5.0/whatsnew
            services.AddDbContextPool<GenVCareContext>(options =>
            {
                options.UseSqlServer(connectionString);
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });


            // Register all GenWorks endpoints as scoped. These will be passed to requesting controller using dependency injection.
            services.AddScoped<IEventService, EventService>();

            // Add support for Swagger Auto Generation
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("GenWorksAPI",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = Configuration.GetSection("OpenAPI").GetSection("Title").Value,
                        Version = Configuration.GetSection("OpenAPI").GetSection("Version").Value,
                        Description = Configuration.GetSection("OpenAPI").GetSection("Description").Value,
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = Configuration.GetSection("OpenAPI").GetSection("ContactEmail").Value,
                            Name = Configuration.GetSection("OpenAPI").GetSection("ContactName").Value,
                            Url = new Uri(Configuration.GetSection("OpenAPI").GetSection("ContactURL").Value)
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = Configuration.GetSection("OpenAPI").GetSection("LicenseType").Value,
                            Url = new Uri(Configuration.GetSection("OpenAPI").GetSection("LicenseURL").Value)
                        }
                    });

                // This helps add appID and locale parameters to each operation
                //setupAction.OperationFilter<CustomHeaderSwaggerAttribute>();
                //setupAction.SchemaFilter<SwaggerExcludeProperty>();


                // TODO: OpenAPI has been setup for basic security scheme but later we need to support bearer as well. This is just sample.
                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" }
                        }, new List<string>() }
                });

                // Project is setup to generate XML documentation using action signatures and XML comments which is loaded dynamically.
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                // Used for including commemts on OpenAPI actions, model and parameters
             // RKG -   setupAction.IncludeXmlComments(xmlCommentsFullPath);

                // This is to ensure if two actions have same URI and verb then which one should be used in OpenAPI documentation.
                // This is typically used for method response variation based on vendor specific header and versioning.
                setupAction.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                setupAction.IgnoreObsoleteActions();
                setupAction.IgnoreObsoleteProperties();
                setupAction.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();

            //Enable middleware to serve generated swagger as a JSON endpoint
            app.UseSwagger();

            //Enable middleware to serve swagger-ui (HTML, JS, CSS, rtc...),
            //specifying the swagger json endpoint
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/GenWorksAPI/swagger.json",
                    "Gen Works API");
                setupAction.RoutePrefix = string.Empty;

                setupAction.DefaultModelExpandDepth(2);
                setupAction.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);

                setupAction.EnableDeepLinking();
                setupAction.DisplayOperationId();
                setupAction.DocExpansion(DocExpansion.None);
                setupAction.DisplayRequestDuration();
                setupAction.EnableFilter();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
