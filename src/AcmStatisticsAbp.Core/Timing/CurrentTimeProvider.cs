// <copyright file="CurrentTimeProvider.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Timing
{
    using System;
    using Abp.Dependency;

    /// <summary>
    /// 提供当前的时间
    /// </summary>
    public class CurrentTimeProvider : ITimeProvider, ISingletonDependency
    {
        public DateTime Now => DateTime.Now;
    }
}
