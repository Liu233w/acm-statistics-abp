// <copyright file="AcmStatisticsAbpAuthorizationProvider.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Authorization
{
    using Abp.Authorization;
    using Abp.Localization;
    using Abp.MultiTenancy;

    public class AcmStatisticsAbpAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Settings, L("Settings"));

            context.CreatePermission(PermissionNames.Pages_WorkerUsername, new FixedLocalizableString("用户名记录"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AcmStatisticsAbpConsts.LocalizationSourceName);
        }
    }
}
