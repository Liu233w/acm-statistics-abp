// <copyright file="AccountAppService.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Authorization.Accounts
{
    using System.Threading.Tasks;
    using Abp.Configuration;
    using Abp.UI;
    using Abp.Zero.Configuration;
    using AcmStatisticsAbp.Authorization.Accounts.Dto;
    using AcmStatisticsAbp.Authorization.EmailConfirmation;
    using AcmStatisticsAbp.Authorization.Users;

    public class AccountAppService : AcmStatisticsAbpAppServiceBase, IAccountAppService
    {
        private readonly UserRegistrationManager userRegistrationManager;
        private readonly EmailConfirmationManager emailConfirmationManager;
        private readonly UserManager userManager;

        public AccountAppService(
            UserRegistrationManager userRegistrationManager, EmailConfirmationManager emailConfirmationManager, UserManager userManager)
        {
            this.userRegistrationManager = userRegistrationManager;
            this.emailConfirmationManager = emailConfirmationManager;
            this.userManager = userManager;
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
                await this.emailConfirmationManager.SendConfirmationEmailAsync(user);
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

        /// <summary>
        /// 重复发送验证邮件。用于用户没有收到邮件的情况。在已经验证了邮箱的情况下，会报错
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="UserFriendlyException">在验证了邮箱的情况下，会报错</exception>
        public async Task SendEmailConfirmLink(SendEmailConfirmLinkInput input)
        {
            var user = await this.userManager.FindByNameOrEmailAsync(input.UsernameOrEmail);

            await this.emailConfirmationManager.SendConfirmationEmailAsync(user);
        }
    }
}
