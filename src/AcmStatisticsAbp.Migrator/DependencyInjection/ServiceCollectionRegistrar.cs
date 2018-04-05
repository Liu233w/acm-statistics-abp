namespace AcmStatisticsAbp.Migrator.DependencyInjection
{
    using Abp.Dependency;
    using AcmStatisticsAbp.Identity;
    using Castle.Windsor.MsDependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionRegistrar
    {
        public static void Register(IIocManager iocManager)
        {
            var services = new ServiceCollection();

            IdentityRegistrar.Register(services);

            WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);
        }
    }
}
