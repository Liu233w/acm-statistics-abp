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

            // 邮件的部分设置（推荐使用yandex邮箱，这个不需要手机号就能注册，并且国内外都能发；163邮箱发送的邮件没法让 Outlook 邮箱接收）
            this.AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "NWPU-ACM 查询系统 邮件机器人");
            this.AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "npuacm@yandex.com");
            this.AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, "npuacm");
            this.AddSettingIfNotExists(EmailSettingNames.Smtp.Password, "----邮箱密码----");
            this.AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "smtp.yandex.com");
            this.AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl, "true");
            this.AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, "false");
            this.AddSettingIfNotExists(EmailSettingNames.Smtp.Port, "25");

            // Languages
            this.AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en");
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
