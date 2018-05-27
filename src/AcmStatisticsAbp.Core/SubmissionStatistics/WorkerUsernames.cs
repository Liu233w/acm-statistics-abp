// <copyright file="WorkerUsernames.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.SubmissionStatistics
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Abp.Domain.Entities;
    using Abp.Domain.Entities.Auditing;
    using AcmStatisticsAbp.Authorization.Users;

    /// <summary>
    /// 存储了某个用户在查题网站上的用户名，某个用户可以有多个 Set，方便切换
    /// </summary>
    [Table("worker_usernames")]
    public class WorkerUsernames : FullAuditedEntity<long, User>, IExtendableObject
    {
        public string ExtensionData { get; set; }

        /// <summary>
        /// Gets or sets 使用此 Set 的用户名
        /// </summary>
        [Required]
        public User User { get; set; }

        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets 该订阅的名称，显示给用户，方便用户检索
        /// </summary>
        [MaxLength(128)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets 用户在各个网站上的用户名
        /// </summary>
        [NotMapped]
        public Usernames Usernames
        {
            get => this.GetData<Usernames>("usernames");
            set => this.SetData("usernames", value);
        }

        /// <summary>
        /// Gets or sets 此用户名列表的所有订阅，可以为空（没有订阅）
        /// </summary>
        public ICollection<CrawlerSubscription> CrawlerSubscriptions { get; set; }
    }
}
