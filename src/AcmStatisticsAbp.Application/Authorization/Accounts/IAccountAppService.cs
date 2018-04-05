using System.Threading.Tasks;
using Abp.Application.Services;
using AcmStatisticsAbp.Authorization.Accounts.Dto;

namespace AcmStatisticsAbp.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
