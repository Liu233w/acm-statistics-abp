// <copyright file="IAdminSettingAppService.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Configuration
{
    using System.Threading.Tasks;
    using Abp.Application.Services;
    using Abp.Application.Services.Dto;
    using AcmStatisticsAbp.Configuration.Dto;

    public interface IAdminSettingAppService : IApplicationService
    {
        Task ChangeApplicationSetting(ChangeApplicationSettingInput input);

        Task<ListResultDto<ListAllApplicationSettingsOutputItem>> ListAllApplicationSettings();
    }
}
