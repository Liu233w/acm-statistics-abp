// <copyright file="AuthenticateModel.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Models.TokenAuth
{
    using System.ComponentModel.DataAnnotations;
    using Abp.Auditing;
    using Abp.Authorization.Users;

    public class AuthenticateModel
    {
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool RememberClient { get; set; }
    }
}
