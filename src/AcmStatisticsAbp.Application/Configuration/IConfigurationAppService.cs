using System.Threading.Tasks;
using AcmStatisticsAbp.Configuration.Dto;

namespace AcmStatisticsAbp.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
