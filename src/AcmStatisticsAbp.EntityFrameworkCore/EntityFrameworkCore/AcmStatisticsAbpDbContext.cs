// <copyright file="AcmStatisticsAbpDbContext.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.EntityFrameworkCore
{
    using Abp.Zero.EntityFrameworkCore;
    using AcmStatisticsAbp.Authorization.EmailConfirmation;
    using AcmStatisticsAbp.Authorization.Roles;
    using AcmStatisticsAbp.Authorization.Users;
    using AcmStatisticsAbp.MultiTenancy;
    using AcmStatisticsAbp.SubmissionStatistics;
    using Microsoft.EntityFrameworkCore;

    public class AcmStatisticsAbpDbContext : AbpZeroDbContext<Tenant, Role, User, AcmStatisticsAbpDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<ConfirmationCode> ConfirmationCodes { get; set; }

        public DbSet<WorkerUsernames> WorkerUsernameses { get; set; }

        public DbSet<CrawlerSubscription> CrawlerSubscriptions { get; set; }

        public AcmStatisticsAbpDbContext(DbContextOptions<AcmStatisticsAbpDbContext> options)
            : base(options)
        {
        }
    }
}
