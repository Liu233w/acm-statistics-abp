using Abp.AutoMapper;
using AcmStatisticsAbp.Authentication.External;

namespace AcmStatisticsAbp.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
