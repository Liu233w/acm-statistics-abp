// <copyright file="PredefinedMailPeriod.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.SubmissionStatistics
{
    /// <summary>
    /// 发送邮件的周期
    /// </summary>
    public static class PredefinedMailPeriod
    {
        /// <summary>
        /// 每天早上 8 点
        /// </summary>
        public const string Daily = "0 8 * * *";

        /// <summary>
        /// 每周周一早上 8 点
        /// </summary>
        public const string Weekly = "0 8 * * Mon";

        /// <summary>
        /// 每月 1 日早上 8 点
        /// </summary>
        public const string Monthly = "0 8 1 * *";
    }
}
