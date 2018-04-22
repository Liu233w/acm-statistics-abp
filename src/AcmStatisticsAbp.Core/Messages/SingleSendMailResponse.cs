// <copyright file="SingleSendMailResponse.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

/*
 * 本文件来自于阿里巴巴SDK源代码，https://github.com/aliyun/aliyun-openapi-net-sdk
 * Copyright 2009-2018 Alibaba Cloud All rights reserved.
 */

namespace AcmStatisticsAbp.Messages
{
    using Aliyun.Acs.Core;

    public class SingleSendMailResponse : AcsResponse
    {
        public string EnvId { get; set; }
    }
}
