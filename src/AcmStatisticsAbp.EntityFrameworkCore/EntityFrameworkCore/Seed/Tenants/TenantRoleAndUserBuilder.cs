// <copyright file="TenantRoleAndUserBuilder.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
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
        private readonly AcmStatisticsAbpDbContext context;
        private readonly int tenantId;

        public TenantRoleAndUserBuilder(AcmStatisticsAbpDbContext context, int tenantId)
        {
            this.context = context;
            this.tenantId = tenantId;
        }

        public void Create()
        {
            this.CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            // Admin role
            var adminRole = this.context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == this.tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = this.context.Roles.Add(new Role(this.tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                this.context.SaveChanges();
            }

            // Grant all permissions to admin role
            var grantedPermissions = this.context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == this.tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new AcmStatisticsAbpAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                this.context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = this.tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id,
                    }));
                this.context.SaveChanges();
            }

            // Admin user
            var adminUser = this.context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == this.tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(this.tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                this.context.Users.Add(adminUser);
                this.context.SaveChanges();

                // Assign Admin role to admin user
                this.context.UserRoles.Add(new UserRole(this.tenantId, adminUser.Id, adminRole.Id));
                this.context.SaveChanges();
            }

            // NormalUser role
            var normalUserRole = this.CreateAndGetRoleIfNotExist(StaticRoleNames.Tenants.NormalUser);
            this.GrantPermissionForRules(
                normalUserRole,
                PermissionNames.Pages_WorkerUsername);
        }

        private Role CreateAndGetRoleIfNotExist(string roleName)
        {
            var role = this.context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == this.tenantId && r.Name == roleName);
            if (role == null)
            {
                role = this.context.Roles.Add(
                    new Role(this.tenantId, roleName, roleName)
                    {
                        IsStatic = true,
                    }).Entity;
                this.context.SaveChanges();
            }

            return role;
        }

        private void GrantPermissionForRules(Role role, params string[] permissionNames)
        {
            var granted = this.context.RolePermissions
                .Where(item => item.TenantId == this.tenantId && item.RoleId == role.Id)
                .Select(item => item.Name);

            var shouldGrant = permissionNames.Except(granted);
            foreach (var name in shouldGrant)
            {
                this.context.Permissions.Add(new RolePermissionSetting
                {
                    RoleId = role.Id,
                    TenantId = this.tenantId,
                    IsGranted = true,
                    Name = name,
                });
            }

            this.context.SaveChanges();
        }
    }
}
