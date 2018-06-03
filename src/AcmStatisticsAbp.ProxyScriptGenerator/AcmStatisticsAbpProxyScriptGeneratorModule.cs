// <copyright file="AcmStatisticsAbpProxyScriptGeneratorModule.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.ProxyScriptGenerator
{
    using Abp.AspNetCore.Configuration;
    using Abp.Events.Bus;
    using Abp.Modules;
    using Abp.Reflection.Extensions;
    using AcmStatisticsAbp.EntityFrameworkCore;
    using Castle.MicroKernel.Registration;
    using Microsoft.AspNetCore.Builder;

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

            this.Configuration.IocManager.Resolve<IAbpAspNetCoreConfiguration>().RouteConfiguration.Add(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public override void Initialize()
        {
            this.IocManager.RegisterAssemblyByConvention(typeof(AcmStatisticsAbpProxyScriptGeneratorModule).GetAssembly());
        }
    }
}
