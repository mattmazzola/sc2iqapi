using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Authentication.JwtBearer;
using jwtTest.Options;
using jwtTest.Infrastructure.Messaging;
using Newtonsoft.Json.Serialization;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace jwtTest
{
    public class Startup
    {
        const string TokenAudience = "ExampleAudience";
        const string TokenIssuer = "ExampleIssuer";
        private RsaSecurityKey key;
        private TokenAuthOptions tokenOptions;

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            if (env.IsEnvironment("Development"))
            {
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");

            var serviceBusNamespace = Configuration.Get<string>("messaging:namespace");
            var serviceBusManageKey = Configuration.Get<string>("ServiceBusManageKey");
            var contexts = Configuration.Get<List<string>>("messaging:contexts");
            var defaultSubscriptionName = Configuration.Get<string>("defaultSubscriptionName");

            // Setup Messaging Infrastructure
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("rootmanage", serviceBusManageKey);
            var nameSpaceClient = new NamespaceManager(ServiceBusEnvironment.CreateServiceUri("sb", serviceBusNamespace, string.Empty), tokenProvider);

            contexts.ForEach(context =>
            {
                var commandsTopicName = $"{context}/commands";
                var eventsTopicName = $"{context}/events";
                var defaultTopicDescription = new TopicDescription(string.Empty)
                {
                    DefaultMessageTimeToLive = TimeSpan.FromDays(1),
                    DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(10),
                    RequiresDuplicateDetection = true
                };

                if (!nameSpaceClient.TopicExists(commandsTopicName))
                {
                    var commandTopicDescription = defaultTopicDescription;
                    commandTopicDescription.Path = commandsTopicName;
                    nameSpaceClient.CreateTopic(commandTopicDescription);
                }

                if (!nameSpaceClient.SubscriptionExists(commandsTopicName, defaultSubscriptionName))
                {
                    var commandSubscriptionDescription = new SubscriptionDescription(commandsTopicName, defaultSubscriptionName);
                    nameSpaceClient.CreateSubscription(commandSubscriptionDescription);
                }

                if (!nameSpaceClient.TopicExists(eventsTopicName))
                {
                    var eventsTopicDescription = defaultTopicDescription;
                    eventsTopicDescription.Path = eventsTopicName;
                    nameSpaceClient.CreateTopic(eventsTopicName);
                }

                // TODO: Get list of all event types per context and create subscription for each of them.
                if (!nameSpaceClient.SubscriptionExists(eventsTopicName, defaultSubscriptionName))
                {
                    var eventsSubscriptionDescription = new SubscriptionDescription(eventsTopicName, defaultSubscriptionName);
                    nameSpaceClient.CreateSubscription(eventsSubscriptionDescription);
                }
            });
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // *** CHANGE THIS FOR PRODUCTION USE ***
            // Here, we're generating a random key to sign tokens - obviously this means
            // that each time the app is started the key will change, and multiple servers 
            // all have different keys. This should be changed to load a key from a file 
            // securely delivered to your application, controlled by configuration.
            //
            // See the RSAKeyUtils.GetKeyParameters method for an examle of loading from
            // a JSON file.
            RSAParameters keyParams = RSAKeyUtils.GetRandomKey();

            // Create the key, and a set of token options to record signing credentials 
            // using that key, along with the other parameters we will need in the 
            // token controlller.
            key = new RsaSecurityKey(keyParams);
            tokenOptions = new TokenAuthOptions()
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature)
            };

            services.AddInstance<TokenAuthOptions>(tokenOptions);

            // Enable the use of an [Authorize("Bearer")] attribute on methods and classes to protect.
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc()
                .AddJsonOptions(opts =>
             {
                 opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
             });

            services.AddOptions();
            services.Configure<ServiceBusOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseJwtBearerAuthentication(options =>
            {
                // Basic settings - signing key to validate with, audience and issuer.
                options.TokenValidationParameters.IssuerSigningKey = key;
                options.TokenValidationParameters.ValidAudience = tokenOptions.Audience;
                options.TokenValidationParameters.ValidIssuer = tokenOptions.Issuer;

                // When receiving a token, check that we've signed it.
                options.TokenValidationParameters.ValidateSignature = true;

                // When receiving a token, check that it is still valid.
                options.TokenValidationParameters.ValidateLifetime = true;

                // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                // machines which should have synchronised time, this can be set to zero. Where external tokens are
                // used, some leeway here could be useful.
                options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(0);
            });

            app.UseStaticFiles();

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
