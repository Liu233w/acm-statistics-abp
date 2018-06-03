// <copyright file="Startup.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.ProxyScriptGenerator
{
    using System;
    using Abp.AspNetCore;
    using Abp.AspNetCore.Configuration;
    using Abp.AspNetCore.Mvc.Extensions;
    using Abp.Reflection.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            var mvc = services.AddMvc();
            mvc.PartManager.ApplicationParts
                .Add(new AssemblyPart(typeof(AbpAspNetCoreModule).GetAssembly()));

            // Configure Abp and Dependency Injection
            return services.AddAbp<AcmStatisticsAbpProxyScriptGeneratorModule>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp();

            app.UseMvc(routes =>
            {
                app.ApplicationServices.GetRequiredService<IAbpAspNetCoreConfiguration>().RouteConfiguration.ConfigureAll(routes);
            });
        }
    }
}
