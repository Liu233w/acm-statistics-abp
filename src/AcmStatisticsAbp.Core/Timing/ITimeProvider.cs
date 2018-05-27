// <copyright file="ITimeProvider.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Timing
{
    using System;

    /// <summary>
    /// 用于提供当前时间的接口，使用此接口来获取时间（而不是 DateTime.Now）以便于单元测试
    /// </summary>
    public interface ITimeProvider
    {
        DateTime Now { get; }
    }
}
