namespace AcmStatisticsAbp.Authorization.Accounts.Dto
{
    using System.ComponentModel.DataAnnotations;
    using Abp.MultiTenancy;

    public class IsTenantAvailableInput
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
    }
}
