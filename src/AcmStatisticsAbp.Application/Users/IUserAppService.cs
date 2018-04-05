using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AcmStatisticsAbp.Roles.Dto;
using AcmStatisticsAbp.Users.Dto;

namespace AcmStatisticsAbp.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
