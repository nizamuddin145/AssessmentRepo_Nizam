using System.Web.Http;
using System.Web.Http.Filters;
using Common;
using Core;
using Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Raven.Client.Documents.Session;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using WebApi.App_Start;
using WebApi.Controllers;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new Container();
            var lifestyle = Lifestyle.Scoped;
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            var assembly = typeof(WebApiConfig).Assembly;
            InitializeAssemblyInstancesService.Initialize(container, lifestyle, assembly);

            DataConfiguration.Initialize(container, lifestyle);
            CoreConfiguration.Initialize(container, lifestyle);

            // Register ILogger
            container.Register<ILoggerFactory>(() =>
                new LoggerFactory(), Lifestyle.Singleton);

            container.Register<ILogger<UserController>>(
                () => container.GetInstance<ILoggerFactory>().CreateLogger<UserController>(),
                Lifestyle.Scoped);
            container.Register<ILogger<ProductController>>(
                () => container.GetInstance<ILoggerFactory>().CreateLogger<ProductController>(),
                Lifestyle.Scoped);
            container.Register<ILogger<OrderController>>(
                () => container.GetInstance<ILoggerFactory>().CreateLogger<OrderController>(),
                Lifestyle.Scoped);

            container.RegisterWebApiControllers(config);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = config.DependencyResolver;

            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.DateParseHandling = DateParseHandling.DateTime;
            settings.DefaultValueHandling = DefaultValueHandling.Include;
            settings.Converters.Add(new StringEnumConverter());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                                       "DefaultApi",
                                       "api/{controller}/{id}",
                                       new { id = RouteParameter.Optional }
                                      );

            config.Filters.AddRange(new IFilter[]
                                    {
                                        new GlobalExceptionFilter(),
                                        new ContextInitializeAttribute()                                        
                                    });
        }
    }
}