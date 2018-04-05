namespace AcmStatisticsAbp.Configuration
{
    using System.Threading.Tasks;
    using Abp.Authorization;
    using Abp.Runtime.Session;
    using AcmStatisticsAbp.Configuration.Dto;

    [AbpAuthorize]
    public class ConfigurationAppService : AcmStatisticsAbpAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
