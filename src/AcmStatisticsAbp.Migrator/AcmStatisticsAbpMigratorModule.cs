using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AcmStatisticsAbp.Configuration;
using AcmStatisticsAbp.EntityFrameworkCore;
using AcmStatisticsAbp.Migrator.DependencyInjection;

namespace AcmStatisticsAbp.Migrator
{
    [DependsOn(typeof(AcmStatisticsAbpEntityFrameworkModule))]
    public class AcmStatisticsAbpMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AcmStatisticsAbpMigratorModule(AcmStatisticsAbpEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(AcmStatisticsAbpMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                AcmStatisticsAbpConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AcmStatisticsAbpMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
