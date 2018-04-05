namespace AcmStatisticsAbp.Sessions
{
    using System.Threading.Tasks;
    using Abp.Application.Services;
    using AcmStatisticsAbp.Sessions.Dto;

    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
