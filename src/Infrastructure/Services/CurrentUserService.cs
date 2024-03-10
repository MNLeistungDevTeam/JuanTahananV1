using DMS.Domain.Dto.UserDto;
using Microsoft.AspNetCore.Http;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.RoleDto;
using DMS.Domain.Entities;
using DMS.Application.Interfaces.Setup.RoleRepository;

namespace DMS.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    
    }

    public int GetCurrentUserId()
    {
        int userId = int.Parse(_httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "");
        return userId;
    }

    public async Task<UserModel?> GetUserInfo()
    {
        var userId = GetCurrentUserId();
        var data = await _userRepository.GetUserByIdAsync(userId);
        return data;
    }

     

}