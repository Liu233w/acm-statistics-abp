// <copyright file="Usernames.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.SubmissionStatistics
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 记录用户在各个 OJ 上的用户名以及主用户名
    /// </summary>
    public class Usernames
    {
        /// <summary>
        /// Gets or sets 主用户名，如果在查询的时候副用户名不存在，就用这个来替换
        /// </summary>
        [MaxLength(128)]
        public string MainUsername { get; set; }

        /// <summary>
        /// Gets or sets 用户在各个 OJ 上的用户名。
        ///
        /// 如果某一项不存在，而 config.yml 中存在，会使用 MainUsername 来替换。如果不需要查询某个爬虫，需要将对应的 value 设置成空字符串。
        /// </summary>
        public ICollection<NameForWorker> SubUsernames { get; set; }

        public class NameForWorker
        {
            /// <summary>
            /// Gets or sets 爬虫的名称
            /// </summary>
            public string CrawlerName { get; set; }

            /// <summary>
            /// Gets or sets 用户在该爬虫上的用户名
            /// </summary>
            public string Username { get; set; }
        }
    }
}
