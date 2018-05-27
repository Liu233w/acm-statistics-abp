// <copyright file="AcmStatisticsAbpControllerBase.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Controllers
{
    using Abp.AspNetCore.Mvc.Controllers;
    using Abp.IdentityFramework;
    using Microsoft.AspNetCore.Identity;

    public abstract class AcmStatisticsAbpControllerBase : AbpController
    {
        protected AcmStatisticsAbpControllerBase()
        {
            this.LocalizationSourceName = AcmStatisticsAbpConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(this.LocalizationManager);
        }
    }
}
