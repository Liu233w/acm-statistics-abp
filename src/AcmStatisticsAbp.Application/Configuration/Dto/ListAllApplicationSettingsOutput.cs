// <copyright file="ListAllApplicationSettingsOutput.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Configuration.Dto
{
    using System.Collections.Generic;
    using Abp.Configuration;

    public class ListAllApplicationSettingsOutput
    {
        public IReadOnlyList<ISettingValue> Settings { get; set; }
    }
}
