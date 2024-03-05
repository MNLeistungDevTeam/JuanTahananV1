using DMS.Domain.Dto.UserDto;

namespace DMS.Application.Services;

public interface ICurrentUserService
{
    int GetCurrentUserId();

    Task<UserModel?> GetUserInfo();
}