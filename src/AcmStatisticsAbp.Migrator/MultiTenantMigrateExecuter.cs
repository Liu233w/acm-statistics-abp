// <copyright file="MultiTenantMigrateExecuter.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Migrator
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Abp.Data;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Extensions;
    using Abp.MultiTenancy;
    using Abp.Runtime.Security;
    using AcmStatisticsAbp.EntityFrameworkCore;
    using AcmStatisticsAbp.EntityFrameworkCore.Seed;
    using AcmStatisticsAbp.MultiTenancy;

    public class MultiTenantMigrateExecuter : ITransientDependency
    {
        private readonly Log _log;
        private readonly AbpZeroDbMigrator _migrator;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IDbPerTenantConnectionStringResolver _connectionStringResolver;

        public MultiTenantMigrateExecuter(
            AbpZeroDbMigrator migrator,
            IRepository<Tenant> tenantRepository,
            Log log,
            IDbPerTenantConnectionStringResolver connectionStringResolver)
        {
            this._log = log;

            this._migrator = migrator;
            this._tenantRepository = tenantRepository;
            this._connectionStringResolver = connectionStringResolver;
        }

        public bool Run(bool skipConnVerification)
        {
            var hostConnStr = CensorConnectionString(this._connectionStringResolver.GetNameOrConnectionString(new ConnectionStringResolveArgs(MultiTenancySides.Host)));
            if (hostConnStr.IsNullOrWhiteSpace())
            {
                this._log.Write("Configuration file should contain a connection string named 'Default'");
                return false;
            }

            this._log.Write("Host database: " + ConnectionStringHelper.GetConnectionString(hostConnStr));
            if (!skipConnVerification)
            {
                this._log.Write("Continue to migration for this host database and all tenants..? (Y/N): ");
                var command = Console.ReadLine();
                if (!command.IsIn("Y", "y"))
                {
                    this._log.Write("Migration canceled.");
                    return false;
                }
            }

            this._log.Write("HOST database migration started...");

            try
            {
                this._migrator.CreateOrMigrateForHost(SeedHelper.SeedHostDb);
            }
            catch (Exception ex)
            {
                this._log.Write("An error occured during migration of host database:");
                this._log.Write(ex.ToString());
                this._log.Write("Canceled migrations.");
                return false;
            }

            this._log.Write("HOST database migration completed.");
            this._log.Write("--------------------------------------------------------");

            var migratedDatabases = new HashSet<string>();
            var tenants = this._tenantRepository.GetAllList(t => t.ConnectionString != null && t.ConnectionString != "");
            for (var i = 0; i < tenants.Count; i++)
            {
                var tenant = tenants[i];
                this._log.Write(string.Format("Tenant database migration started... ({0} / {1})", (i + 1), tenants.Count));
                this._log.Write("Name              : " + tenant.Name);
                this._log.Write("TenancyName       : " + tenant.TenancyName);
                this._log.Write("Tenant Id         : " + tenant.Id);
                this._log.Write("Connection string : " + SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString));

                if (!migratedDatabases.Contains(tenant.ConnectionString))
                {
                    try
                    {
                        this._migrator.CreateOrMigrateForTenant(tenant);
                    }
                    catch (Exception ex)
                    {
                        this._log.Write("An error occured during migration of tenant database:");
                        this._log.Write(ex.ToString());
                        this._log.Write("Skipped this tenant and will continue for others...");
                    }

                    migratedDatabases.Add(tenant.ConnectionString);
                }
                else
                {
                    this._log.Write("This database has already migrated before (you have more than one tenant in same database). Skipping it....");
                }

                this._log.Write(string.Format("Tenant database migration completed. ({0} / {1})", (i + 1), tenants.Count));
                this._log.Write("--------------------------------------------------------");
            }

            this._log.Write("All databases have been migrated.");

            return true;
        }

        private static string CensorConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            var keysToMask = new[] { "password", "pwd", "user id", "uid" };

            foreach (var key in keysToMask)
            {
                if (builder.ContainsKey(key))
                {
                    builder[key] = "*****";
                }
            }

            return builder.ToString();
        }
    }
}
