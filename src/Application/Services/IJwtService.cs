using DMS.Domain.Dto.Authentication;
using DMS.Domain.Entities;

namespace DMS.Application.Services;

public interface IJwtService
{
    Task<User> Authenticate(string token);
    Task<AuthResponse> GetRefreshTokenAsync(int userId);

    Task<AuthResponse?> GetTokenAsync(AuthRequest authRequest);

    Task<bool> IsTokenValid(string accessToken);
}