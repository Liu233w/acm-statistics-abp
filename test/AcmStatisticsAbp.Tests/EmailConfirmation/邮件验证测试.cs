// <copyright file="邮件验证测试.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Tests.EmailConfirmation
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Abp.Authorization;
    using Abp.Extensions;
    using Abp.Net.Mail;
    using AcmStatisticsAbp.Authorization;
    using AcmStatisticsAbp.Authorization.Accounts;
    using AcmStatisticsAbp.Authorization.Accounts.Dto;
    using AcmStatisticsAbp.Authorization.EmailConfirmation;
    using Moq;
    using Shouldly;
    using Xbehave;

    public class 邮件验证测试 : AcmStatisticsAbpTestBase
    {
        private const string SampleEmail = "simple@test.com";

        private readonly IAccountAppService accountAppService;
        private readonly LogInManager logInManager;

        private string emailBody;

        public 邮件验证测试()
        {
            Mock<IEmailSender> emailSenderMock = new Mock<IEmailSender>();

            var emailConfirmationManager = this.Resolve<EmailConfirmationManager>(new
            {
                emailSender = emailSenderMock.Object,
            });

            this.accountAppService = this.Resolve<AccountAppService>(new
            {
                emailConfirmationManager,
            });

            this.logInManager = this.Resolve<LogInManager>();

            // Mock 邮件方法
            emailSenderMock.Setup(
                    obj => obj.SendAsync(
                        SampleEmail,
                        It.Is<string>(item => !item.IsNullOrEmpty()),
                        It.IsAny<string>(),
                        true))
                .Returns(Task.FromResult(0))
                .Callback(new Action<string, string, string, bool>((x, y, body, z) => { this.emailBody = body; }))
                .Verifiable();
        }

        [Scenario]
        public void 主要路线(RegisterOutput output, string confirmToken)
        {
            "有用户 sample 注册了新账户".x(async () =>
            {
                output = await this.accountAppService.Register(new RegisterInput
                {
                    EmailAddress = SampleEmail,
                    UserName = "sample",
                    Name = "sample",
                    Surname = "test",
                    Password = "123qwe",
                });
            });

            "此时 sample 无法登录".x(async () =>
            {
                output.CanLogin.ShouldBe(false);

                (await this.logInManager.LoginAsync(SampleEmail, "123qwe"))
                    .Result.ShouldBe(AbpLoginResultType.UserEmailIsNotConfirmed);
            });

            "sample 应该收到一封邮件，含有邮件认证地址".x(() =>
            {
                this.emailBody.ShouldNotBeNullOrEmpty();

                var regex = new Regex("(?<=<a href=\").*(?=\">)");
                var confirmUrl = regex.Match(this.emailBody).Value;
                confirmToken = confirmUrl.Split('/').Last();
            });

            "sample 使用此链接来验证邮件地址".x(async () =>
            {
                await this.accountAppService.ConfirmEmail(new ConfirmEmailInput
                {
                    ConfirmationToken = confirmToken,
                });
            });

            "现在 sample 可以登录了".x(async () =>
            {
                (await this.logInManager.LoginAsync(SampleEmail, "123qwe"))
                    .Result.ShouldBe(AbpLoginResultType.Success);
            });
        }
    }
}
