// <copyright file="Program.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.ProxyScriptGenerator
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Abp;
    using Abp.Dependency;
    using Abp.Web.Api.ProxyScripting;
    using Abp.Web.Api.ProxyScripting.Generators.JQuery;
    using AcmStatisticsAbp.ProxyScripting;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet run -- [output path]");
            }

            var host = BuildWebHost(new string[0]);
            await host.StartAsync();

            var bootstrapper = host.Services.GetService(typeof(AbpBootstrapper)) as AbpBootstrapper ?? throw new Exception("AbpBootstrapper 不存在");
            var apiProxyScriptManager = bootstrapper.IocManager.Resolve<IApiProxyScriptManager>();

            var script = apiProxyScriptManager.GetScript(new ApiProxyGenerationOptions(AxiosProxyScriptGenerator.Name, false));
            File.WriteAllText(args[0], script);

            await host.StopAsync();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddCommandLine(args)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(config)
                .Build();
        }
    }
}
