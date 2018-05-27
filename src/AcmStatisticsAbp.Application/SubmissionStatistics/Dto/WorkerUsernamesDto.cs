// <copyright file="WorkerUsernamesDto.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.SubmissionStatistics.Dto
{
    using System.ComponentModel.DataAnnotations;
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    [AutoMap(typeof(WorkerUsernames))]
    public class WorkerUsernamesDto : IEntityDto<long>
    {
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets 该订阅的名称，显示给用户，方便用户检索
        /// </summary>
        [MaxLength(128)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets 用户名
        /// </summary>
        // 这里不使用 Dto，因为本来就是一个 POCO 而不是 Entity
        public Usernames Usernames { get; set; }

        public long Id { get; set; }
    }
}
