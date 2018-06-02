// <copyright file="AcmStatisticsAbpProxyScriptGeneratorModule.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.ProxyScriptGenerator
{
    using Abp.Configuration.Startup;
    using Abp.Events.Bus;
    using Abp.Modules;
    using Abp.Reflection.Extensions;
    using Abp.Web.Api.ProxyScripting;
    using Abp.Web.Api.ProxyScripting.Configuration;
    using AcmStatisticsAbp.EntityFrameworkCore;
    using Castle.MicroKernel.Registration;

    [DependsOn(typeof(AcmStatisticsAbpWebCoreModule))]
    public class AcmStatisticsAbpProxyScriptGeneratorModule : AbpModule
    {
        private readonly AcmStatisticsAbpEntityFrameworkModule entityFrameworkModule;

        public AcmStatisticsAbpProxyScriptGeneratorModule(AcmStatisticsAbpEntityFrameworkModule entityFrameworkModule)
        {
            this.entityFrameworkModule = entityFrameworkModule;
        }

        public override void PreInitialize()
        {
            this.entityFrameworkModule.SkipDbContextRegistration = true;
            this.entityFrameworkModule.SkipDbSeed = true;

            this.Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            this.Configuration.ReplaceService(
                typeof(IEventBus),
                () => this.IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)));
        }

        public override void Initialize()
        {
            this.IocManager.RegisterAssemblyByConvention(typeof(AcmStatisticsAbpProxyScriptGeneratorModule).GetAssembly());
        }
    }
}
