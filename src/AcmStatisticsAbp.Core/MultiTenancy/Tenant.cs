namespace AcmStatisticsAbp.MultiTenancy
{
    using Abp.MultiTenancy;
    using AcmStatisticsAbp.Authorization.Users;

    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
