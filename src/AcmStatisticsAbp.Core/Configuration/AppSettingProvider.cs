// <copyright file="AppSettingProvider.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Configuration
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Abp.Configuration;
    using Abp.Zero.Configuration;

    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            // 开启邮件验证
            var emailConfirmationSetting =
                context.Manager.GetSettingDefinition(
                    AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
            emailConfirmationSetting.DefaultValue = "true";

            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),

                // 默认发送邮件的域名和协议，结尾不能加上斜杠
                new SettingDefinition(AppSettingNames.EmailConfirmationBaseUrl, "http://new.npuacm.info"),

                // 发送确认邮件的最短间隔，用秒来表示（低于此间隔将不能发送确认邮件，防止发送过于频繁）
                new SettingDefinition(AppSettingNames.MinEmailConfirmationSendInterval, "60"),
            };
        }
    }
}
