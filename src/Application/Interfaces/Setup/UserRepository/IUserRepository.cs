using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.UserRepository;

public interface IUserRepository
{
    Task BatchDeleteAsync(int[] ids);

    Task<User> CreateAsync(User user, int userId);

    Task<List<User>> GetAllAsync();

    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByIdNoTrackingAsync(int id);

    Task<User?> GetByUserNameAsync(string userName);

    Task<UserModel?> GetUserAsync(int id);

    Task<List<UserModel>> GetUserByUserRoleIdAsync(int userRoleId);

    Task<List<UserModel>> GetUsersAsync();

    Task<List<UserModel>> spGetByRoleName(string roleName);

    Task<User?> SaveUserAsync(UserModel user, List<UserApproverModel?> userApprovers, int userId);

    Task<User> UpdateAsync(User user, int userId);
    Task<User> UpdateNoExclusionAsync(User user, int updatedById);
}