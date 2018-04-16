// <copyright file="AdminSettingAppService_Tests.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Tests.Configuration
{
    using System.Threading.Tasks;
    using AcmStatisticsAbp.Configuration;
    using AcmStatisticsAbp.Configuration.Dto;
    using Shouldly;
    using Xunit;

    public class AdminSettingAppService_Tests : AcmStatisticsAbpTestBase
    {
        private readonly IAdminSettingAppService adminSettingAppService;

        public AdminSettingAppService_Tests()
        {
            this.adminSettingAppService = this.Resolve<AdminSettingAppService>();
        }

        [Fact]
        public async Task ListAllApplicationSettings_应当能够返回设置()
        {
            // Act
            var settings = await this.adminSettingAppService.ListAllApplicationSettings();

            // Assert
            settings.Settings.ShouldNotBeEmpty();
            var setting = settings.Settings[0];
            setting.Name.ShouldNotBeNullOrEmpty();
            setting.Value.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task ChangeApplicationSetting_应当能够改变设置()
        {
            // Arrange
            var settings = await this.adminSettingAppService.ListAllApplicationSettings();
            var input = new ChangeApplicationSettingInput
            {
                Name = settings.Settings[0].Name,
                Value = "New Value",
            };

            // Act
            await this.adminSettingAppService.ChangeApplicationSetting(input);

            // Assert
            settings = await this.adminSettingAppService.ListAllApplicationSettings();
            settings.Settings.ShouldContain(item => item.Name == input.Name && item.Value == input.Value);
        }
    }
}
