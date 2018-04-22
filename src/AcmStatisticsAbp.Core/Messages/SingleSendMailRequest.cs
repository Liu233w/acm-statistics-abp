// <copyright file="SingleSendMailRequest.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

/*
 * 本文件来自于阿里巴巴SDK源代码，https://github.com/aliyun/aliyun-openapi-net-sdk
 * Copyright 2009-2018 Alibaba Cloud All rights reserved.
 */

namespace AcmStatisticsAbp.Messages
{
    using Aliyun.Acs.Core;
    using Aliyun.Acs.Core.Http;
    using Aliyun.Acs.Core.Transform;
    using Aliyun.Acs.Core.Utils;

    public class SingleSendMailRequest : RpcAcsRequest<SingleSendMailResponse>
    {
        public SingleSendMailRequest()
            : base("Dm", "2015-11-23", "SingleSendMail")
        {
            this.Method = MethodType.POST;
        }

        private long? ownerId;

        private string resourceOwnerAccount;

        private long? resourceOwnerId;

        private string accountName;

        private int? addressType;

        private string tagName;

        private bool? replyToAddress;

        private string toAddress;

        private string subject;

        private string htmlBody;

        private string textBody;

        private string fromAlias;

        private string replyAddress;

        private string replyAddressAlias;

        private string clickTrace;

        public string ClickTrace
        {
            get => this.clickTrace;
            set
            {
                this.clickTrace = value;
                DictionaryUtil.Add(this.QueryParameters, "ClickTrace", value);
            }
        }

        public long? OwnerId
        {
            get => this.ownerId;
            set
            {
                this.ownerId = value;
                DictionaryUtil.Add(this.QueryParameters, "OwnerId", value.ToString());
            }
        }

        public string ResourceOwnerAccount
        {
            get => this.resourceOwnerAccount;
            set
            {
                this.resourceOwnerAccount = value;
                DictionaryUtil.Add(this.QueryParameters, "ResourceOwnerAccount", value);
            }
        }

        public long? ResourceOwnerId
        {
            get => this.resourceOwnerId;
            set
            {
                this.resourceOwnerId = value;
                DictionaryUtil.Add(this.QueryParameters, "ResourceOwnerId", value.ToString());
            }
        }

        public string AccountName
        {
            get => this.accountName;
            set
            {
                this.accountName = value;
                DictionaryUtil.Add(this.QueryParameters, "AccountName", value);
            }
        }

        public int? AddressType
        {
            get => this.addressType;
            set
            {
                this.addressType = value;
                DictionaryUtil.Add(this.QueryParameters, "AddressType", value.ToString());
            }
        }

        public string TagName
        {
            get => this.tagName;
            set
            {
                this.tagName = value;
                DictionaryUtil.Add(this.QueryParameters, "TagName", value);
            }
        }

        public bool? ReplyToAddress
        {
            get => this.replyToAddress;
            set
            {
                this.replyToAddress = value;
                DictionaryUtil.Add(this.QueryParameters, "ReplyToAddress", value.ToString());
            }
        }

        public string ToAddress
        {
            get => this.toAddress;
            set
            {
                this.toAddress = value;
                DictionaryUtil.Add(this.QueryParameters, "ToAddress", value);
            }
        }

        public string Subject
        {
            get => this.subject;
            set
            {
                this.subject = value;
                DictionaryUtil.Add(this.QueryParameters, "Subject", value);
            }
        }

        public string HtmlBody
        {
            get => this.htmlBody;
            set
            {
                this.htmlBody = value;
                DictionaryUtil.Add(this.QueryParameters, "HtmlBody", value);
            }
        }

        public string TextBody
        {
            get => this.textBody;
            set
            {
                this.textBody = value;
                DictionaryUtil.Add(this.QueryParameters, "TextBody", value);
            }
        }

        public string FromAlias
        {
            get => this.fromAlias;
            set
            {
                this.fromAlias = value;
                DictionaryUtil.Add(this.QueryParameters, "FromAlias", value);
            }
        }

        public string ReplyAddress
        {
            get => this.replyAddress;
            set
            {
                this.replyAddress = value;
                DictionaryUtil.Add(this.QueryParameters, "ReplyAddress", value);
            }
        }

        public string ReplyAddressAlias
        {
            get => this.replyAddressAlias;
            set
            {
                this.replyAddressAlias = value;
                DictionaryUtil.Add(this.QueryParameters, "ReplyAddressAlias", value);
            }
        }

        public override SingleSendMailResponse GetResponse(UnmarshallerContext unmarshallerContext)
        {
            // 目前不需要 Response，就不实现此接口了
            // TODO: 实现一个实际的 Response 接口
            return new SingleSendMailResponse();
        }
    }
}
