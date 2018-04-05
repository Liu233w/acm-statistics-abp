namespace AcmStatisticsAbp.Configuration
{
    using System.Threading.Tasks;
    using AcmStatisticsAbp.Configuration.Dto;

    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
