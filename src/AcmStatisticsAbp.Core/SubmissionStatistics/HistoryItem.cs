// <copyright file="HistoryItem.cs" company="西北工业大学ACM技术组">
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
    /// 查询的历史记录
    /// </summary>
    [Table("crawler_histories")]
    public class HistoryItem : CreationAuditedEntity<long, User>
    {
        public int Solved { get; set; }

        public int Submissions { get; set; }

        /// <summary>
        /// Gets or sets 将历史记录关联到某个用户，表示这是某个用户的查询结果
        /// </summary>
        [Required]
        public User User { get; set; }

        public long UserId { get; set; }

        public ICollection<QueryDetail> QueryDetails { get; set; }
    }
}
