// <copyright file="CrawlerSubscription.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.SubmissionStatistics
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Abp.Domain.Entities.Auditing;
    using AcmStatisticsAbp.Authorization.Users;

    /// <summary>
    /// 订阅。用户可以订阅某些 CrawlerUsernameSet，用来定期给用户发送报告
    /// </summary>
    [Table("crawler_subscriptions")]
    public class CrawlerSubscription : CreationAuditedEntity<long, User>
    {
        /// <summary>
        /// Gets or sets 多久发送一次邮件，是 crontab 表达式。频率不能超过每日一次。
        /// </summary>
        [Required]
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets 跟订阅关联的 CrawlerUsernameSet
        /// </summary>
        [Required]
        public CrawlerUsernameSet CrawlerUsernameSet { get; set; }

        public long CrawlerUsernameSetId { get; set; }
    }
}
