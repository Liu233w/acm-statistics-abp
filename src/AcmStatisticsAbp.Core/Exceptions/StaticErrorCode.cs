// <copyright file="StaticErrorCode.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Exceptions
{
    public class StaticErrorCode
    {
        public const int UserNotFound = 1;

        public const int EmailAlreadyConfirmed = 2;

        public const int ConfirmCodeNotFound = 3;

        /// <summary>
        /// 确认邮件的发送太频繁
        /// </summary>
        public const int ConfirmEmailTooFrequent = 4;

        public const int EntityNotFound = 5;
    }
}
