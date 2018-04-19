// <copyright file="邮件验证测试.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Tests.EmailConfirmation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Abp.Authorization;
    using Abp.Extensions;
    using Abp.Net.Mail;
    using Abp.UI;
    using AcmStatisticsAbp.Authorization;
    using AcmStatisticsAbp.Authorization.Accounts;
    using AcmStatisticsAbp.Authorization.Accounts.Dto;
    using AcmStatisticsAbp.Authorization.EmailConfirmation;
    using AcmStatisticsAbp.Tests.DependencyInjection;
    using Moq;
    using Shouldly;
    using Xbehave;

    public class 邮件验证测试 : AcmStatisticsAbpTestBase
    {
        private const string SampleEmail = "simple@test.com";

        private readonly IAccountAppService accountAppService;
        private readonly LogInManager logInManager;

        private readonly List<string> emailBodys;

        private readonly Mock<IEmailSender> emailSenderMock;

        private readonly CustomTimeProvider customTimeProvider;

        public 邮件验证测试()
        {
            this.emailSenderMock = new Mock<IEmailSender>();
            this.customTimeProvider = new CustomTimeProvider();

            var emailConfirmationManager = this.Resolve<EmailConfirmationManager>(new
            {
                emailSender = this.emailSenderMock.Object,
                timeProvider = this.customTimeProvider,
            });

            this.accountAppService = this.Resolve<AccountAppService>(new
            {
                emailConfirmationManager,
            });

            this.logInManager = this.Resolve<LogInManager>();

            this.emailBodys = new List<string>();

            // Mock 邮件方法
            this.emailSenderMock.Setup(
                    obj => obj.SendAsync(
                        SampleEmail,
                        It.Is<string>(item => !item.IsNullOrEmpty()),
                        It.IsAny<string>(),
                        true))
                .Returns(Task.FromResult(0))
                .Callback(new Action<string, string, string, bool>((x, y, body, z) => { this.emailBodys.Add(body); }))
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
                this.emailBodys.ShouldNotBeEmpty();
                this.emailBodys[0].ShouldNotBeNullOrEmpty();

                var regex = new Regex("(?<=<a href=\").*(?=\">)");
                var confirmUrl = regex.Match(this.emailBodys[0]).Value;
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

        [Scenario]
        public void 用户在验证邮箱之前多次发送验证信息(RegisterOutput output, string confirmToken, DateTime currentTime)
        {
            "确定当前的时间".x(() =>
            {
                currentTime = DateTime.Now;
                this.customTimeProvider.TimeFunc = () => currentTime;
            });

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

            "时间过去了1分钟以上".x(() => { currentTime = currentTime.AddMinutes(2); });

            "sample 再次发送验证邮件".x(async () =>
            {
                await this.accountAppService.SendEmailConfirmLink(new SendEmailConfirmLinkInput
                {
                    UsernameOrEmail = "sample",
                });
            });

            "sample 应该收到两封邮件，其内容相同".x(() =>
            {
                this.emailSenderMock.Verify(
                    sender => sender.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true),
                    Moq.Times.Exactly(2));

                this.emailBodys.Count.ShouldBe(2);

                this.emailBodys[0].ShouldBe(this.emailBodys[1]);
            });

            "sample 收到的邮件中应含有邮件认证地址".x(() =>
            {
                this.emailBodys[0].ShouldNotBeNullOrEmpty();

                var regex = new Regex("(?<=<a href=\").*(?=\">)");
                var confirmUrl = regex.Match(this.emailBodys[0]).Value;
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

        [Scenario]
        public void 用户在验证成功之后重复单击验证链接(RegisterOutput output, string confirmToken, Task confirmTask)
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
                this.emailBodys.ShouldNotBeEmpty();
                this.emailBodys[0].ShouldNotBeNullOrEmpty();

                var regex = new Regex("(?<=<a href=\").*(?=\">)");
                var confirmUrl = regex.Match(this.emailBodys[0]).Value;
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

            "用户再次验证邮件地址".x(() =>
            {
                confirmTask = this.accountAppService.ConfirmEmail(new ConfirmEmailInput
                {
                    ConfirmationToken = confirmToken,
                });
            });

            "验证会报错".x(async () =>
            {
                var exception = await confirmTask.ShouldThrowAsync<UserFriendlyException>();
                exception.Code.ShouldBe(2);
            });
        }

        [Scenario]
        public void 用户在验证成功之后重复发送验证链接(RegisterOutput output, string confirmToken, Task sendEmailTask, DateTime currentTime)
        {
            "确定当前的时间".x(() =>
            {
                currentTime = DateTime.Now;
                this.customTimeProvider.TimeFunc = () => currentTime;
            });

            "有用户 sample 注册了新账户".x(async () =>
            {
                output = await this.accountAppService.Register(new RegisterInput
                {
                    EmailAddress = SampleEmail,
                    UserName = "sample",
                    Name = "sam",
                    Surname = "ple",
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
                this.emailBodys.ShouldNotBeEmpty();
                this.emailBodys[0].ShouldNotBeNullOrEmpty();

                var regex = new Regex("(?<=<a href=\").*(?=\">)");
                var confirmUrl = regex.Match(this.emailBodys[0]).Value;
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

            "时间过去了1分钟以上".x(() => { currentTime = currentTime.AddMinutes(2); });

            "用户再次发送验证邮件".x(() =>
            {
                sendEmailTask = this.accountAppService.SendEmailConfirmLink(new SendEmailConfirmLinkInput
                {
                    UsernameOrEmail = "sample",
                });
            });

            "验证会报错".x(async () =>
            {
                var exception = await sendEmailTask.ShouldThrowAsync<UserFriendlyException>();
                exception.Code.ShouldBe(2);
            });
        }
    }
}
