namespace AcmStatisticsAbp
{
    using Abp.AutoMapper;
    using Abp.Modules;
    using Abp.Reflection.Extensions;
    using AcmStatisticsAbp.Authorization;

    [DependsOn(
        typeof(AcmStatisticsAbpCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class AcmStatisticsAbpApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AcmStatisticsAbpAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(AcmStatisticsAbpApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
