// <copyright file="CustomTimeProvider.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Tests.DependencyInjection
{
    using System;
    using AcmStatisticsAbp.Timing;

    /// <summary>
    /// 可以自行设置当前的时间
    /// </summary>
    public class CustomTimeProvider : ITimeProvider
    {
        public Func<DateTime> TimeFunc { get; set; } = () => DateTime.Now;

        public DateTime Now => this.TimeFunc();
    }
}
