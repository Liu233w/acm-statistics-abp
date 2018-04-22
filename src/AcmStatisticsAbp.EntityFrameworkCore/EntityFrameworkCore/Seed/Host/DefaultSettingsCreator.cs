// <copyright file="DefaultSettingsCreator.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.EntityFrameworkCore.Seed.Host
{
    using System.Linq;
    using Abp.Configuration;
    using Abp.Localization;
    using Abp.Net.Mail;
    using Abp.Zero.Configuration;
    using AcmStatisticsAbp.Configuration;
    using Microsoft.EntityFrameworkCore;

    public class DefaultSettingsCreator
    {
        private readonly AcmStatisticsAbpDbContext context;

        public DefaultSettingsCreator(AcmStatisticsAbpDbContext context)
        {
            this.context = context;
        }

        public void Create()
        {
            // 开启邮件验证
            this.AddSettingIfNotExists(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, "true");

            // 邮件的部分设置，本项目使用的是阿里云邮件的 SDK
            this.AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "邮件机器人");
            this.AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "bot@notice.npuacm.info");
            this.AddSettingIfNotExists(AppSettingNames.AliYunEmailReplyToAddress, "true");
            this.AddSettingIfNotExists(AppSettingNames.AliYunEmailAccessKeyId, "id");
            this.AddSettingIfNotExists(AppSettingNames.AliYunEmailAccessSecret, "secret");
            this.AddSettingIfNotExists(AppSettingNames.EmailConfirmationBaseUrl, "http://new.npuacm.info");

            // Languages
            this.AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "zh-CN");
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (this.context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            this.context.Settings.Add(new Setting(tenantId, null, name, value));
            this.context.SaveChanges();
        }
    }
}
