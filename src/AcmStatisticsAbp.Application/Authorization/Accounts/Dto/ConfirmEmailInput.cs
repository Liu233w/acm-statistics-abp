// <copyright file="ConfirmEmailInput.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
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
