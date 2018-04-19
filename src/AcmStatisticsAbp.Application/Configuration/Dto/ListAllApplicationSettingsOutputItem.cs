// <copyright file="ListAllApplicationSettingsOutputItem.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Configuration.Dto
{
    using System.ComponentModel.DataAnnotations;
    using Abp.AutoMapper;
    using Abp.Configuration;

    [AutoMapFrom(typeof(SettingDefinition))]
    public class ListAllApplicationSettingsOutputItem
    {
        [Required]
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Gets or sets 当前的值，如果为默认值，返回 null
        /// </summary>
        public string Value { get; set; }

        public string DefaultValue { get; set; }
    }
}
