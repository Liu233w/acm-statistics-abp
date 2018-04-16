// <copyright file="AdminSettingAppService.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Configuration
{
    using System.Threading.Tasks;
    using Abp.Authorization;
    using Abp.Configuration;
    using AcmStatisticsAbp.Authorization;
    using AcmStatisticsAbp.Configuration.Dto;

    [AbpAuthorize(PermissionNames.Pages_Settings)]
    public class AdminSettingAppService : AcmStatisticsAbpAppServiceBase, IAdminSettingAppService
    {
        private readonly ISettingManager settingManager;

        public AdminSettingAppService(ISettingManager settingManager)
        {
            this.settingManager = settingManager;
        }

        /// <summary>
        /// 列出所有的程序设置，包括名称和值
        /// </summary>
        public async Task<ListAllApplicationSettingsOutput> ListAllApplicationSettings()
        {
            return new ListAllApplicationSettingsOutput
            {
                Settings = await this.settingManager.GetAllSettingValuesForApplicationAsync(),
            };
        }

        /// <summary>
        /// 更改程序设置
        /// </summary>
        /// <param name="input"></param>
        public async Task ChangeApplicationSetting(ChangeApplicationSettingInput input)
        {
            await this.settingManager.ChangeSettingForApplicationAsync(input.Name, input.Value);
        }
    }
}
