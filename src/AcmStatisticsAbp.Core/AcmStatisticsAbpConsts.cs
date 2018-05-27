// <copyright file="AcmStatisticsAbpConsts.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp
{
    public static class AcmStatisticsAbpConsts
    {
        public const string LocalizationSourceName = "AcmStatisticsAbp";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;

        /// <summary>
        /// 邮箱验证的 URI，这个由前端决定，0处是验证用的地址
        /// </summary>
        public const string EmailConfirmationUri = "/email-confirmation/{0}";
    }
}
