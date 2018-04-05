using Abp.Authorization;
using AcmStatisticsAbp.Authorization.Roles;
using AcmStatisticsAbp.Authorization.Users;

namespace AcmStatisticsAbp.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
