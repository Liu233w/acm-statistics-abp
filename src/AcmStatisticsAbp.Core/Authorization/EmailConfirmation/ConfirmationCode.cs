// <copyright file="ConfirmationCode.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Authorization.EmailConfirmation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Abp.Domain.Entities.Auditing;
    using AcmStatisticsAbp.Authorization.Users;

    /// <summary>
    /// 表示一个唯一的值，用来放在邮件里发送给用户，从而验证用户的邮箱地址
    /// </summary>
    [Table("confirmation_codes")]
    public class ConfirmationCode : CreationAuditedEntity<Guid, User>
    {
        [Required]
        public User User { get; set; }

        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets 上次发送邮件的时间
        /// </summary>
        [Required]
        public DateTime LastSendTime { get; set; }
    }
}
