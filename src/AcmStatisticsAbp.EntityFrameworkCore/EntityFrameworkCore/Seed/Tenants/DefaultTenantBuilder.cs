// <copyright file="DefaultTenantBuilder.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.EntityFrameworkCore.Seed.Tenants
{
    using System.Linq;
    using Abp.MultiTenancy;
    using AcmStatisticsAbp.Editions;
    using AcmStatisticsAbp.MultiTenancy;
    using Microsoft.EntityFrameworkCore;

    public class DefaultTenantBuilder
    {
        private readonly AcmStatisticsAbpDbContext _context;

        public DefaultTenantBuilder(AcmStatisticsAbpDbContext context)
        {
            this._context = context;
        }

        public void Create()
        {
            this.CreateDefaultTenant();
        }

        private void CreateDefaultTenant()
        {
            // Default tenant

            var defaultTenant = this._context.Tenants.IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == AbpTenantBase.DefaultTenantName);
            if (defaultTenant == null)
            {
                defaultTenant = new Tenant(AbpTenantBase.DefaultTenantName, AbpTenantBase.DefaultTenantName);

                var defaultEdition = this._context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                if (defaultEdition != null)
                {
                    defaultTenant.EditionId = defaultEdition.Id;
                }

                this._context.Tenants.Add(defaultTenant);
                this._context.SaveChanges();
            }
        }
    }
}
