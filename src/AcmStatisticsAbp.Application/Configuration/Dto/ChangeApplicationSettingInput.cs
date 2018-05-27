// <copyright file="ChangeApplicationSettingInput.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Configuration.Dto
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeApplicationSettingInput
    {
        /// <summary>
        /// Gets or sets 设置名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets 设置的值
        /// </summary>
        public string Value { get; set; }
    }
}
