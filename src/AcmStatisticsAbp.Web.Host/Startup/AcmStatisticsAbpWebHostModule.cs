// <copyright file="AcmStatisticsAbpWebHostModule.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Web.Host.Startup
{
    using Abp.Modules;
    using Abp.Reflection.Extensions;
    using AcmStatisticsAbp.Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    [DependsOn(
       typeof(AcmStatisticsAbpWebCoreModule))]
    public class AcmStatisticsAbpWebHostModule : AbpModule
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly IHostingEnvironment env;

        // ReSharper disable once NotAccessedField.Local
        private readonly IConfigurationRoot appConfiguration;

        public AcmStatisticsAbpWebHostModule(IHostingEnvironment env)
        {
            this.env = env;
            this.appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            this.IocManager.RegisterAssemblyByConvention(typeof(AcmStatisticsAbpWebHostModule).GetAssembly());
        }
    }
}
