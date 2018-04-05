// <copyright file="TenantRoleAndUserBuilder.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.EntityFrameworkCore.Seed.Tenants
{
    using System.Linq;
    using Abp.Authorization;
    using Abp.Authorization.Roles;
    using Abp.Authorization.Users;
    using Abp.MultiTenancy;
    using AcmStatisticsAbp.Authorization;
    using AcmStatisticsAbp.Authorization.Roles;
    using AcmStatisticsAbp.Authorization.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class TenantRoleAndUserBuilder
    {
        private readonly AcmStatisticsAbpDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(AcmStatisticsAbpDbContext context, int tenantId)
        {
            this._context = context;
            this._tenantId = tenantId;
        }

        public void Create()
        {
            this.CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            // Admin role

            var adminRole = this._context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == this._tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = this._context.Roles.Add(new Role(this._tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                this._context.SaveChanges();
            }

            // Grant all permissions to admin role

            var grantedPermissions = this._context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == this._tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new AcmStatisticsAbpAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                this._context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = this._tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    }));
                this._context.SaveChanges();
            }

            // Admin user

            var adminUser = this._context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == this._tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(this._tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                this._context.Users.Add(adminUser);
                this._context.SaveChanges();

                // Assign Admin role to admin user
                this._context.UserRoles.Add(new UserRole(this._tenantId, adminUser.Id, adminRole.Id));
                this._context.SaveChanges();

                // User account of admin user
                if (this._tenantId == 1)
                {
                    this._context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = this._tenantId,
                        UserId = adminUser.Id,
                        UserName = AbpUserBase.AdminUserName,
                        EmailAddress = adminUser.EmailAddress
                    });
                    this._context.SaveChanges();
                }
            }
        }
    }
}
