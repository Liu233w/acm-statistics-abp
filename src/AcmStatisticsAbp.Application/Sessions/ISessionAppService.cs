using System.Threading.Tasks;
using Abp.Application.Services;
using AcmStatisticsAbp.Sessions.Dto;

namespace AcmStatisticsAbp.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
