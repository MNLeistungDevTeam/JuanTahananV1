using DMS.Domain.Dto.RoleDto;
using DMS.Domain.Dto.UserDto;

namespace DMS.Application.Services;

public interface ICurrentUserService
{
    int GetCurrentUserId();
    Task<RoleAccessModel> GetRoleAccess(string moduleCode);
    Task<UserModel?> GetUserInfo();
}