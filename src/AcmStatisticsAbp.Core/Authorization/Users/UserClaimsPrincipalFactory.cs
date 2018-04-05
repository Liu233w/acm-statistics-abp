namespace AcmStatisticsAbp.Authorization.Users
{
    using Abp.Authorization;
    using AcmStatisticsAbp.Authorization.Roles;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;

    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor)
        {
        }
    }
}
