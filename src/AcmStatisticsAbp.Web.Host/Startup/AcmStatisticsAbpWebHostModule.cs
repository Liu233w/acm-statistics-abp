namespace AcmStatisticsAbp.Web.Host.Startup
{
    using Abp.Modules;
    using Abp.Reflection.Extensions;
    using AcmStatisticsAbp.Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    [DependsOn(
       typeof(AcmStatisticsAbpWebCoreModule))]
    public class AcmStatisticsAbpWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AcmStatisticsAbpWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AcmStatisticsAbpWebHostModule).GetAssembly());
        }
    }
}
