using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AcmStatisticsAbp.MultiTenancy.Dto;

namespace AcmStatisticsAbp.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
