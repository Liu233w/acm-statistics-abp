namespace AcmStatisticsAbp.Authorization.Roles
{
    using Abp.Authorization.Roles;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using AcmStatisticsAbp.Authorization.Users;

    public class RoleStore : AbpRoleStore<Role, User>
    {
        public RoleStore(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Role> roleRepository,
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository)
            : base(
                unitOfWorkManager,
                roleRepository,
                rolePermissionSettingRepository)
        {
        }
    }
}
