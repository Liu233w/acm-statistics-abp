using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using AcmStatisticsAbp.Authorization.Roles;
using AcmStatisticsAbp.Authorization.Users;
using AcmStatisticsAbp.MultiTenancy;

namespace AcmStatisticsAbp.EntityFrameworkCore
{
    public class AcmStatisticsAbpDbContext : AbpZeroDbContext<Tenant, Role, User, AcmStatisticsAbpDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public AcmStatisticsAbpDbContext(DbContextOptions<AcmStatisticsAbpDbContext> options)
            : base(options)
        {
        }
    }
}
