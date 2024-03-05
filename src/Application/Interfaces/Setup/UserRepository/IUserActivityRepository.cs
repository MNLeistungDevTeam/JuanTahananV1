using DMS.Domain.Entities;
using DMS.Domain.Dto.UserDto;

namespace DMS.Application.Interfaces.Setup.UserRepository;

public interface IUserActivityRepository
{
    Task<UserActivity> CreateAsync(UserActivity userActivity);

    Task<List<UserActivityModel>> GetUserActivities(DateTime? dateFrom, DateTime? dateTo);

    Task<List<UserActivityModel>> GetUserActivitiesByUserId(int userId);

    Task<UserActivityModel?> GetUserActivityById(int id);
}