namespace AcmStatisticsAbp.Sessions.Dto
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;
    using AcmStatisticsAbp.MultiTenancy;

    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
