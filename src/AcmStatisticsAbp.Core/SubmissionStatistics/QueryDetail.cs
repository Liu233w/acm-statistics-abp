// <copyright file="QueryDetail.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.SubmissionStatistics
{
    using JetBrains.Annotations;

    /// <summary>
    /// 某次查询的详细记录，包含了某个爬虫的查询结果
    /// </summary>
    public class QueryDetail
    {
        /// <summary>
        /// Gets or sets 爬虫的名称（name字段）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets 当次爬取所使用的用户名
        /// </summary>
        public string Username { get; set; }

        public int Solved { get; set; }

        public int Submissions { get; set; }

        /// <summary>
        /// Gets or sets 错误信息。如果当次查询没有得到结果（报错），这里存储错误信息。如果此处为空，代表查询得到了正确结果。
        /// </summary>
        [CanBeNull]
        public string ErrorMessage { get; set; }
    }
}
