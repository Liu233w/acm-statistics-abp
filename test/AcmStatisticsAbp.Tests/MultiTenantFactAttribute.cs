// <copyright file="MultiTenantFactAttribute.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Tests
{
    using Xunit;

    public sealed class MultiTenantFactAttribute : FactAttribute
    {
        public MultiTenantFactAttribute()
        {
            if (!AcmStatisticsAbpConsts.MultiTenancyEnabled)
            {
                this.Skip = "MultiTenancy is disabled.";
            }
        }
    }
}
