using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace AcmStatisticsAbp.Controllers
{
    public abstract class AcmStatisticsAbpControllerBase: AbpController
    {
        protected AcmStatisticsAbpControllerBase()
        {
            LocalizationSourceName = AcmStatisticsAbpConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
