namespace AcmStatisticsAbp.Authentication.External
{
    using System.Collections.Generic;
    using Abp.Dependency;

    public class ExternalAuthConfiguration : IExternalAuthConfiguration, ISingletonDependency
    {
        public List<ExternalLoginProviderInfo> Providers { get; }

        public ExternalAuthConfiguration()
        {
            Providers = new List<ExternalLoginProviderInfo>();
        }
    }
}
