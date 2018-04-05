namespace AcmStatisticsAbp.Models.TokenAuth
{
    using Abp.AutoMapper;
    using AcmStatisticsAbp.Authentication.External;

    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
