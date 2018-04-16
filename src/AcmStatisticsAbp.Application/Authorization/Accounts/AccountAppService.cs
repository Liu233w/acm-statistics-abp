// <copyright file="AccountAppService.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Authorization.Accounts
{
    using System.Threading.Tasks;
    using Abp.Configuration;
    using Abp.Zero.Configuration;
    using AcmStatisticsAbp.Authorization.Accounts.Dto;
    using AcmStatisticsAbp.Authorization.EmailConfirmation;
    using AcmStatisticsAbp.Authorization.Users;

    public class AccountAppService : AcmStatisticsAbpAppServiceBase, IAccountAppService
    {
        private readonly UserRegistrationManager userRegistrationManager;
        private readonly EmailConfirmationManager emailConfirmationManager;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager, EmailConfirmationManager emailConfirmationManager)
        {
            this.userRegistrationManager = userRegistrationManager;
            this.emailConfirmationManager = emailConfirmationManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await this.TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var user = await this.userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                false);

            var isEmailConfirmationRequiredForLogin = await this.SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            if (isEmailConfirmationRequiredForLogin)
            {
                await this.emailConfirmationManager.SendConfirmationEmailAsync(input.EmailAddress);
            }

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin),
            };
        }

        public async Task ConfirmEmail(ConfirmEmailInput input)
        {
            await this.emailConfirmationManager.TryConfirmEmailAsync(input.ConfirmationToken);
        }
    }
}
