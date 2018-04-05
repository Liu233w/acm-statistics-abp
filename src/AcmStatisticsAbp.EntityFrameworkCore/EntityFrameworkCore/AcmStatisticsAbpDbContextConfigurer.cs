using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace AcmStatisticsAbp.EntityFrameworkCore
{
    public static class AcmStatisticsAbpDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AcmStatisticsAbpDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AcmStatisticsAbpDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}