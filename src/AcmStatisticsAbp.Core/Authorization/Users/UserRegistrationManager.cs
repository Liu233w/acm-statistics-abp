// <copyright file="UserRegistrationManager.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Authorization.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Abp.Authorization.Users;
    using Abp.Domain.Services;
    using Abp.IdentityFramework;
    using Abp.Runtime.Session;
    using Abp.UI;
    using AcmStatisticsAbp.Authorization.Roles;
    using AcmStatisticsAbp.MultiTenancy;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserRegistrationManager : DomainService
    {
        public IAbpSession AbpSession { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRegistrationManager(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher)
        {
            this._tenantManager = tenantManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._passwordHasher = passwordHasher;

            this.AbpSession = NullAbpSession.Instance;
        }

        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed)
        {
            this.CheckForTenant();

            var tenant = await this.GetActiveTenantAsync();

            var user = new User
            {
                TenantId = tenant.Id,
                Name = name,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = true,
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            user.Password = this._passwordHasher.HashPassword(user, plainPassword);

            foreach (var defaultRole in await this._roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
            {
                user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
            }

            this.CheckErrors(await this._userManager.CreateAsync(user));
            await this.CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        private void CheckForTenant()
        {
            if (!this.AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Can not register host users!");
            }
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!this.AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await this.GetActiveTenantAsync(this.AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await this._tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(this.L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(this.L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(this.LocalizationManager);
        }
    }
}
