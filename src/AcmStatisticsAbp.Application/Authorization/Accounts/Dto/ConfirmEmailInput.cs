// <copyright file="ConfirmEmailInput.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Authorization.Accounts.Dto
{
    using System.ComponentModel.DataAnnotations;

    public class ConfirmEmailInput
    {
        [Required]
        public string ConfirmationToken { get; set; }
    }
}
