using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using AcmStatisticsAbp.Configuration;
using AcmStatisticsAbp.Web;

namespace AcmStatisticsAbp.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class AcmStatisticsAbpDbContextFactory : IDesignTimeDbContextFactory<AcmStatisticsAbpDbContext>
    {
        public AcmStatisticsAbpDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AcmStatisticsAbpDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            AcmStatisticsAbpDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AcmStatisticsAbpConsts.ConnectionStringName));

            return new AcmStatisticsAbpDbContext(builder.Options);
        }
    }
}
