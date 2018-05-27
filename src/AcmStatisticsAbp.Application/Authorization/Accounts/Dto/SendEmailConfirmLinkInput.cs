// <copyright file="SendEmailConfirmLinkInput.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Authorization.Accounts.Dto
{
    public class SendEmailConfirmLinkInput
    {
        public string UsernameOrEmail { get; set; }
    }
}
