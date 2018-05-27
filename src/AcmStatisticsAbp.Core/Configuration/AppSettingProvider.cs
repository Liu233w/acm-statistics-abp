// <copyright file="AppSettingProvider.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Configuration
{
    using System.Collections.Generic;
    using Abp.Configuration;
    using Abp.Localization;

    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),

                new SettingDefinition(
                    AppSettingNames.EmailConfirmationBaseUrl,
                    "http://localhost:12005",
                    description: new FixedLocalizableString("默认发送邮件的域名和协议，结尾不能加上斜杠")),

                new SettingDefinition(
                    AppSettingNames.MinEmailConfirmationSendInterval,
                    "60",
                    description: new FixedLocalizableString("发送确认邮件的最短间隔，用秒来表示（低于此间隔将不能发送确认邮件，防止发送过于频繁）")),

                new SettingDefinition(
                    AppSettingNames.AliYunEmailAccessKeyId,
                    string.Empty,
                    displayName: new FixedLocalizableString("阿里云邮件的签名Id，参见 https://help.aliyun.com/document_detail/29442.html")),

                new SettingDefinition(
                    AppSettingNames.AliYunEmailAccessSecret,
                    string.Empty,
                    displayName: new FixedLocalizableString("阿里云邮件的签名Secret，参见 https://help.aliyun.com/document_detail/29442.html")),

                new SettingDefinition(
                    AppSettingNames.AliYunEmailReplyToAddress,
                    "false",
                    displayName: new FixedLocalizableString("是否允许用户回复邮件"),
                    description: new FixedLocalizableString("如果允许，需要在阿里云管理控制台设置回信地址")),
            };
        }
    }
}
