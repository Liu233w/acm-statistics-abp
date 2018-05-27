// <copyright file="EmailConfirmationManager.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Authorization.EmailConfirmation
{
    using System;
    using System.Diagnostics;
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
    using AcmStatisticsAbp.Timing;

    /// <inheritdoc />
    public class EmailConfirmationManager : ITransientDependency
    {
        private readonly IEmailSender emailSender;
        private readonly IRepository<ConfirmationCode, Guid> confirmationCodeRepository;
        private readonly ISettingManager settingManager;
        private readonly ITimeProvider timeProvider;

        public EmailConfirmationManager(IEmailSender emailSender, IRepository<ConfirmationCode, Guid> confirmationCodeRepository, ISettingManager settingManager, ITimeProvider timeProvider)
        {
            this.emailSender = emailSender;
            this.confirmationCodeRepository = confirmationCodeRepository;
            this.settingManager = settingManager;
            this.timeProvider = timeProvider;
        }

        /// <summary>
        /// 生成链接并向用户发送确认邮件，如果之前已经生成过链接，则复用此链接，如果用户已经确认过邮件，则抛出 UserFirendlyException
        /// </summary>
        /// <param name="user">用户对象，必须有ID</param>
        public async Task SendConfirmationEmailAsync(User user)
        {
            Debug.Assert(user.Id != 0, "user 的 Id 必须存在");

            if (user.IsEmailConfirmed)
            {
                throw new UserFriendlyException(StaticErrorCode.EmailAlreadyConfirmed, "此用户已经验证过地址");
            }

            var confirmCode = await this.confirmationCodeRepository.FirstOrDefaultAsync(item => item.UserId == user.Id);

            if (confirmCode != null)
            {
                var currentTime = this.timeProvider.Now;
                var minEmailIntervalSetting =
                    await this.settingManager.GetSettingValueAsync(AppSettingNames.MinEmailConfirmationSendInterval);
                var minInterval = TimeSpan.FromSeconds(int.Parse(minEmailIntervalSetting));
                if (currentTime - confirmCode.LastSendTime <= minInterval)
                {
                    throw new UserFriendlyException(StaticErrorCode.ConfirmEmailTooFrequent, $"确认邮件发送太过频繁，请在{minEmailIntervalSetting}分钟之后再请求发送。");
                }

                confirmCode.LastSendTime = currentTime;
            }
            else
            {
                confirmCode = new ConfirmationCode
                {
                    UserId = user.Id,
                    LastSendTime = this.timeProvider.Now,
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
        /// 验证用户。并将用户设为已验证状态。假如出现错误（例如没有找到验证码、用户已验证）会直接抛出异常。
        /// </summary>
        /// <param name="confirmationToken"></param>
        public async Task<User> ConfirmEmailAndGetUserAsync(string confirmationToken)
        {
            Guid confirmId;
            try
            {
                confirmId = new Guid(confirmationToken);
            }
            catch (FormatException)
            {
                throw new UserFriendlyException(StaticErrorCode.ConfirmCodeNotFound, "未找到此确认码");
            }

            var confirmCode = await this.confirmationCodeRepository.FirstOrDefaultAsync(confirmId);
            if (confirmCode == null)
            {
                throw new UserFriendlyException(StaticErrorCode.ConfirmCodeNotFound, "未找到此确认码");
            }

            var ctx = this.confirmationCodeRepository.GetDbContext();
            await ctx.Entry(confirmCode).Reference(item => item.User).LoadAsync();

            if (confirmCode.User.IsEmailConfirmed)
            {
                throw new UserFriendlyException(StaticErrorCode.EmailAlreadyConfirmed, "您已验证过此邮箱，不需要重复验证");
            }

            confirmCode.User.IsEmailConfirmed = true;

            await ctx.SaveChangesAsync();

            return confirmCode.User;
        }
    }
}
