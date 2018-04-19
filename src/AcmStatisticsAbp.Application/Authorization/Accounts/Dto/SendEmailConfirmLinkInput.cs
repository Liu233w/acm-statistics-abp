// <copyright file="SendEmailConfirmLinkInput.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Authorization.Accounts.Dto
{
    public class SendEmailConfirmLinkInput
    {
        public string UsernameOrEmail { get; set; }
    }
}
