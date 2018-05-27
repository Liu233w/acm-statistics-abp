// <copyright file="EmailConfirmationManager_Test.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Tests.EmailConfirmation
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Abp.Json;
    using Abp.Net.Mail;
    using Abp.UI;
    using AcmStatisticsAbp.Authorization.EmailConfirmation;
    using AcmStatisticsAbp.Authorization.Users;
    using AcmStatisticsAbp.Tests.DependencyInjection;
    using Bogus;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using Xunit;

    public class EmailConfirmationManager_Test : AcmStatisticsAbpTestBase
    {
        private readonly EmailConfirmationManager emailConfirmationManager;
        private readonly UserRegistrationManager userRegistrationManager;

        private class RegisterUserParam
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }

            public string SurName { get; set; }

            public string Email { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }

            // ReSharper enable UnusedAutoPropertyAccessor.Local
        }

        private Faker<RegisterUserParam> registerUserParamFaker;

        private DateTime timeProviderTime;

        public EmailConfirmationManager_Test()
        {
            var customTimeProvider = new CustomTimeProvider();
            customTimeProvider.TimeFunc = () => this.timeProviderTime;

            this.emailConfirmationManager = this.Resolve<EmailConfirmationManager>(new
            {
                emailSender = this.Resolve<NullEmailSender>(),
                timeProvider = customTimeProvider,
            });

            this.userRegistrationManager = this.Resolve<UserRegistrationManager>();

            this.registerUserParamFaker = new Faker<RegisterUserParam>()
                    .StrictMode(true)
                    .RuleFor(e => e.Name, f => f.Name.FirstName())
                    .RuleFor(e => e.SurName, f => f.Name.LastName())
                    .RuleFor(e => e.Username, f => f.Internet.UserName())
                    .RuleFor(e => e.Email, f => f.Internet.Email())
                    .RuleFor(e => e.Password, f => f.Internet.Password())
                ;

            this.timeProviderTime = DateTime.Now;
        }

        private async Task<User> BuildTestUser()
        {
            var user = this.registerUserParamFaker.Generate();

            User ret = null;
            await this.WithUnitOfWorkAsync(async () =>
            {
                ret = await this.userRegistrationManager.RegisterAsync(
                    user.Name,
                    user.SurName,
                    user.Email,
                    user.Username,
                    user.Password,
                    false);
            });

            return ret;
        }

        [Fact]
        public async Task SendConfirmationEmailAsync_应该生成带有正确用户ID的记录()
        {
            // Arrange
            var user1 = await this.BuildTestUser();
            var user2 = await this.BuildTestUser();

            Debug.Assert(user1.Id != user2.Id, "确保已经生成了用户ID并且ID有效");

            // Act
            await this.WithUnitOfWorkAsync(async () =>
            {
                await this.emailConfirmationManager.SendConfirmationEmailAsync(user1);
                await this.emailConfirmationManager.SendConfirmationEmailAsync(user2);
            });

            // Assert
            await this.UsingDbContextAsync(async ctx =>
            {
                var confirmationCodes = await ctx.ConfirmationCodes.ToListAsync();

                confirmationCodes.Count.ShouldBe(2);
                confirmationCodes[0].Id.ShouldNotBe(confirmationCodes[1].Id);
                confirmationCodes.Select(item => item.UserId).ShouldBe(new[] { user1.Id, user2.Id });
            });
        }

        [Fact]
        public async Task ConfirmEmailAndGetUserAsync_在Token不存在或格式不正确时应当报错()
        {
            await this.WithUnitOfWorkAsync(async () =>
            {
                (await this.emailConfirmationManager.ConfirmEmailAndGetUserAsync("000")
                    .ShouldThrowAsync<UserFriendlyException>()).Code.ShouldBe(3);

                (await this.emailConfirmationManager.ConfirmEmailAndGetUserAsync(default(Guid).ToString())
                    .ShouldThrowAsync<UserFriendlyException>()).Code.ShouldBe(3);
            });
        }

        [Fact]
        public async Task ConfirmEmailAndGetUserAsync_应该能正确返回关联的用户()
        {
            // Arrange
            var user1 = await this.BuildTestUser();
            var confirmCode = this.UsingDbContext(ctx =>
                ctx.ConfirmationCodes.Add(new ConfirmationCode
                {
                    UserId = user1.Id,
                }).Entity);

            // Act
            User user2 = null;
            await this.WithUnitOfWorkAsync(async () =>
            {
                user2 = await this.emailConfirmationManager.ConfirmEmailAndGetUserAsync(confirmCode.Id.ToString());
            });

            // Assert
            user2.Id.ShouldBe(user1.Id);
            user2.UserName.ShouldBe(user1.UserName);
            user2.EmailAddress.ShouldBe(user1.EmailAddress);
        }

        [Fact]
        public async Task 在最短时间间隔内连续发送两封确认邮件时应当报错()
        {
            User user = null;
            await this.WithUnitOfWorkAsync(async () =>
            {
                // Arrange
                user = await this.BuildTestUser();
                await this.emailConfirmationManager.SendConfirmationEmailAsync(user);
            });

            this.timeProviderTime = this.timeProviderTime.AddSeconds(5);

            await this.WithUnitOfWorkAsync(async () =>
            {
                // Act
                var task = this.emailConfirmationManager.SendConfirmationEmailAsync(user);

                // Assert
                var exception = await task.ShouldThrowAsync<UserFriendlyException>();
                exception.Code.ShouldBe(4);
            });
        }
    }
}
