using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Logging;
using Newtonsoft.Json.Serialization;
using sc2iqapi.Options;
using sc2iqapi.Models;
using Microsoft.Data.Entity;

namespace sc2iqapi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.
            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                //.SetBasePath(appEnv.ApplicationBasePath) // beta8
                .AddJsonFile("config.json");

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                //builder.AddApplicationInsightsSettings(developerMode: true);

                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup options with DI
            services.AddOptions();

            // Configure MyOptions using config
            services.Configure<Secrets>(Configuration);

            services.AddCors();
            services.ConfigureCors(options =>
                            options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                .AllowAnyMethod()
                                                                .AllowAnyHeader()));

            services.AddEntityFramework()
                    .AddSqlServer()
                    .AddDbContext<Sc2IqContext>(options => options.UseSqlServer(Configuration["Data:ConnectionString"]));

            // Add Application Insights data collection services to the services container.
            //services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
            //services.AddJsonOptions(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //});

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            services.AddInstance(typeof(IConfiguration), Configuration);
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Add Application Insights to the request pipeline to track HTTP request telemetry data.
            //app.UseApplicationInsightsRequestTelemetry();

            // Track data about exceptions from the application. Should be configured after all error handling middleware in the request pipeline.
            //app.UseApplicationInsightsExceptionTelemetry();

            // Setup CORS to allow any origin since this is an api.
            app.UseCors("AllowAll");

            app.UseRuntimeInfoPage();

            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc();
            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");

            SampleData.Initialize(app.ApplicationServices);
        }
    }
}
