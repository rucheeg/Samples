using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using DeviceSample.API.AutoQuery;
using Funq;
using ServiceStack;
using ServiceStack.Admin;
using ServiceStack.Api.Swagger;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using ServiceStack.Validation;

namespace DeviceSample.API.Host
{
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyAspNet
    public class AppHost : AppHostBase
    {
        //private readonly string _slackIncomingWebHookUrl;
        private readonly bool _debugMode;
        /// <summary>
        /// Base constructor requires a DeviceSampleName and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("AMSMasterInventory", new List<Assembly>()
            { typeof(DeviceSampleQueryService).Assembly
            }.ToArray())
        {
            _debugMode = Boolean.Parse(ConfigurationManager.AppSettings["DebugMode"] ?? "false");
        }
            
        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            JsConfig.IncludeNullValues = true;

            //register a cache client - atm it's storing session in memory 
            //TODO: how will it work when request goes to a different server in HA configuration
            container.Register<ICacheClient>(new MemoryCacheClient());


            //Define Auth Repository
            container.Register<IDbConnectionFactory>(
                c => new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["DCS"].ConnectionString, SqlServerDialect.Provider));

            Plugins.AddRange(new List<IPlugin>()
            {
                new SwaggerFeature(),
                new PostmanFeature(),
                new RequestLogsFeature(),
                new ValidationFeature(),
                new AutoQueryFeature() { MaxLimit = 100 },
                new AdminFeature()
            });
            
            this.GetPlugin<MetadataFeature>().ShowResponseStatusInMetadataPages = true;

            if (_debugMode) SetConfig(new HostConfig { DebugMode = true });
        }

        #region Private Methods
       
        #endregion
    }
}