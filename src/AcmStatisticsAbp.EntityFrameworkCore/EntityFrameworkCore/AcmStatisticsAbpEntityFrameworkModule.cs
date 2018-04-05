namespace AcmStatisticsAbp.EntityFrameworkCore
{
    using Abp.EntityFrameworkCore.Configuration;
    using Abp.Modules;
    using Abp.Reflection.Extensions;
    using Abp.Zero.EntityFrameworkCore;
    using AcmStatisticsAbp.EntityFrameworkCore.Seed;

    [DependsOn(
        typeof(AcmStatisticsAbpCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class AcmStatisticsAbpEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<AcmStatisticsAbpDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        AcmStatisticsAbpDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        AcmStatisticsAbpDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AcmStatisticsAbpEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
