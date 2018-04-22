// <copyright file="AliYunEamilSender.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Messages
{
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Abp.Configuration;
    using Abp.Net.Mail;
    using AcmStatisticsAbp.Configuration;
    using Aliyun.Acs.Core;
    using Aliyun.Acs.Core.Profile;

    /// <summary>
    /// 调用阿里云的API接口，从而发送邮件
    /// </summary>
    public class AliYunEamilSender : IEmailSender
    {
        private readonly ISettingManager settingManager;

        public AliYunEamilSender(ISettingManager settingManager)
        {
            this.settingManager = settingManager;
        }

        public async Task SendAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            if (!isBodyHtml)
            {
                throw new System.NotImplementedException("目前还未实现 bodyHtml = false 的功能");
            }

            var accessId = await this.settingManager.GetSettingValueAsync(AppSettingNames.AliYunEmailAccessKeyId);
            var accessSecret = await this.settingManager.GetSettingValueAsync(AppSettingNames.AliYunEmailAccessSecret);

            // sdk 用法参考 https://help.aliyun.com/document_detail/29461.html
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessId, accessSecret);
            IAcsClient client = new DefaultAcsClient(profile);

            var username = await this.settingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress);
            var canReply = await this.settingManager.GetSettingValueAsync<bool>(AppSettingNames.AliYunEmailReplyToAddress);
            var fromDisplayName =
                await this.settingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName);

            var request = new SingleSendMailRequest
            {
                AccountName = username,
                FromAlias = fromDisplayName,
                AddressType = 1,
                ReplyToAddress = canReply,
                ToAddress = to,
                Subject = subject,
                HtmlBody = body,
            };

            // 忽略 Response
            await Task.Run(() => client.GetAcsResponse(request));
        }

        public void Send(string to, string subject, string body, bool isBodyHtml = true)
        {
            throw new System.NotImplementedException();
        }

        public Task SendAsync(string @from, string to, string subject, string body, bool isBodyHtml = true)
        {
            throw new System.NotImplementedException();
        }

        public void Send(string @from, string to, string subject, string body, bool isBodyHtml = true)
        {
            throw new System.NotImplementedException();
        }

        public void Send(MailMessage mail, bool normalize = true)
        {
            throw new System.NotImplementedException();
        }

        public Task SendAsync(MailMessage mail, bool normalize = true)
        {
            throw new System.NotImplementedException();
        }
    }
}
