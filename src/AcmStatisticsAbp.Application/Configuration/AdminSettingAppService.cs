// <copyright file="AdminSettingAppService.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Abp.Application.Services.Dto;
    using Abp.Authorization;
    using Abp.Collections.Extensions;
    using Abp.Configuration;
    using AcmStatisticsAbp.Authorization;
    using AcmStatisticsAbp.Configuration.Dto;

    [AbpAuthorize(PermissionNames.Pages_Settings)]
    public class AdminSettingAppService : AcmStatisticsAbpAppServiceBase, IAdminSettingAppService
    {
        private readonly ISettingManager settingManager;

        private readonly ISettingDefinitionManager settingDefinitionManager;

        public AdminSettingAppService(ISettingManager settingManager, ISettingDefinitionManager settingDefinitionManager)
        {
            this.settingManager = settingManager;
            this.settingDefinitionManager = settingDefinitionManager;
        }

        /// <summary>
        /// 列出所有的程序设置，包括名称和值
        /// </summary>
        public async Task<ListResultDto<ListAllApplicationSettingsOutputItem>> ListAllApplicationSettings()
        {
            var applicationSettingDefinitions = this.settingDefinitionManager
                .GetAllSettingDefinitions()
                .Where(item => item.Scopes.HasFlag(SettingScopes.Application));
            var settingValueDict = (await this.settingManager.GetAllSettingValuesForApplicationAsync())
                .ToDictionary(settingValue => settingValue.Name, settingValue => settingValue.Value);

            var settings = this.ObjectMapper.Map<List<ListAllApplicationSettingsOutputItem>>(
                applicationSettingDefinitions);

            foreach (var item in settings)
            {
                item.Value = settingValueDict.GetOrDefault(item.Name);
            }

            return new ListResultDto<ListAllApplicationSettingsOutputItem>(settings);
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
