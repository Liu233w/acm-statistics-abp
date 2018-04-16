// <copyright file="EmailConfirmationManager.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Authorization.EmailConfirmation
{
    using System;
    using System.Threading.Tasks;
    using Abp.Configuration;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.EntityFrameworkCore.Repositories;
    using Abp.Extensions;
    using Abp.Net.Mail;
    using Abp.UI;
    using AcmStatisticsAbp.Authorization.Users;
    using AcmStatisticsAbp.Configuration;
    using AcmStatisticsAbp.Exceptions;

    /// <inheritdoc />
    public class EmailConfirmationManager : ITransientDependency
    {
        private readonly IEmailSender emailSender;
        private readonly UserManager userManager;
        private readonly IRepository<ConfirmationCode, Guid> confirmationCodeRepository;
        private readonly ISettingManager settingManager;

        public EmailConfirmationManager(IEmailSender emailSender, UserManager userManager, IRepository<ConfirmationCode, Guid> confirmationCodeRepository, ISettingManager settingManager)
        {
            this.emailSender = emailSender;
            this.userManager = userManager;
            this.confirmationCodeRepository = confirmationCodeRepository;
            this.settingManager = settingManager;
        }

        /// <summary>
        /// 生成链接并向用户发送确认邮件，如果之前已经生成过链接，则复用此链接，如果用户已经确认过邮件，则抛出 UserFirendlyException
        /// </summary>
        /// <param name="emailAddress"></param>
        public async Task SendConfirmationEmailAsync(string emailAddress)
        {
            var user = await this.userManager.FindByEmailAsync(emailAddress);
            if (user == null)
            {
                throw new UserFriendlyException(StaticErrorCode.UserNotFound, "用户不存在");
            }

            if (user.IsEmailConfirmed)
            {
                throw new UserFriendlyException(StaticErrorCode.EmailAlreadyConfirmed, "此用户已经验证过地址");
            }

            var confirmCode = await this.confirmationCodeRepository.FirstOrDefaultAsync(item => item.UserId == user.Id);

            if (confirmCode == null)
            {
                confirmCode = new ConfirmationCode
                {
                    UserId = user.Id,
                };

                confirmCode.Id = await this.confirmationCodeRepository.InsertAndGetIdAsync(confirmCode);
            }

            var confirmationBaseUrl = await this.settingManager.GetSettingValueAsync(AppSettingNames.EmailConfirmationBaseUrl);
            confirmationBaseUrl.RemovePostFix("/");

            var confirmationUrl = confirmationBaseUrl +
                                  string.Format(
                                      AcmStatisticsAbpConsts.EmailConfirmationUri,
                                      confirmCode.Id.ToString());

            await this.emailSender.SendAsync(
                user.EmailAddress,
                "NWPU-ACM 查询系统验证邮件",
                $"请单击下列链接来验证您的邮箱地址：<a href=\"{confirmationUrl}\">{confirmationUrl}</a>");
        }

        /// <summary>
        /// 验证用户。并将用户设为已验证状态。
        /// </summary>
        /// <param name="confirmationToken"></param>
        public async Task TryConfirmEmailAsync(string confirmationToken)
        {
            var confirmId = new Guid(confirmationToken);

            var confirmCode = await this.confirmationCodeRepository.FirstOrDefaultAsync(confirmId);
            if (confirmCode == null)
            {
                throw new UserFriendlyException(StaticErrorCode.ConfirmCodeNotFound, "未找到此确认码");
            }

            var ctx = this.confirmationCodeRepository.GetDbContext();
            await ctx.Entry(confirmCode).Reference(item => item.User).LoadAsync();

            confirmCode.User.IsEmailConfirmed = true;

            await ctx.SaveChangesAsync();
        }
    }
}
