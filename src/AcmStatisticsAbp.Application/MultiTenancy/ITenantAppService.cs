namespace AcmStatisticsAbp.MultiTenancy
{
    using Abp.Application.Services;
    using Abp.Application.Services.Dto;
    using AcmStatisticsAbp.MultiTenancy.Dto;

    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
