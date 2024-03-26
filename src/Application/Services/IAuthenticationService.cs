using DMS.Domain.Dto.Authentication;
using DMS.Domain.Dto.OtherDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;

namespace DMS.Application.Services;

public interface IAuthenticationService
{
    Task<User> Authenticate(AuthRequest authRequest);

    Task InsertUserActivity(int userId, string action);

    Task<User> RegisterUser(UserModel user);

    Task UnlockUser(int id);

    Task<UserModel> UpdateFailedAttempts(string userName);

    Task UpdateOnlineStatus(int id, bool status);

    Task UpdateUserRefreshToken(int userId, string refreshToken, DateTime refreshTokenExpiry);

    Task UserLockedStatus(string userName);

    string GenerateTemporaryPasswordAsync(string name);

    Task<string> GenerateTemporaryUsernameAsync();

    Task<bool> ChangePassword(ChangePasswordModel changePassword);
    string HashPassword(string password, string salt);
    Task<User> ResetCredential(UserModel user);
}