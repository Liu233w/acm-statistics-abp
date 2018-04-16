// <copyright file="AppSettingProvider.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Configuration
{
    using System.Collections.Generic;
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
            };
        }
    }
}
