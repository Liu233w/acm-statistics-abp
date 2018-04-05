using Abp.MultiTenancy;
using AcmStatisticsAbp.Authorization.Users;

namespace AcmStatisticsAbp.MultiTenancy
{
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
