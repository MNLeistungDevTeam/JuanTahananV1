﻿using AutoMapper;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<User> _contextHelper;
    private readonly IUserApproverRepository _userApproverRepo;
    private readonly ISQLDatabaseService _db;
    private readonly IMapper _mapper;

    public UserRepository(
        DMSDBContext context,
        ISQLDatabaseService db,
        IMapper mapper,
        IUserApproverRepository userApproverRepo)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<User>(context);
        _db = db;
        _mapper = mapper;
        _userApproverRepo = userApproverRepo;
    }

    public async Task<User?> GetByIdAsync(int id) =>
        await _contextHelper.GetByIdAsync(id);

    public async Task<User?> GetByIdNoTrackingAsync(int id) =>
        await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User?> GetByUserNameAsync(string userName) =>
        await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

    public async Task<List<User>> GetAllAsync() =>
        await _contextHelper.GetAllAsync();

    public async Task<UserModel?> GetUserAsync(int id) =>
        await _db.LoadSingleAsync<UserModel, dynamic>("spUser_Get", new { id });

    public async Task<List<UserModel>> GetUsersAsync() =>
        (await _db.LoadDataAsync<UserModel, dynamic>("spUser_GetAll", new { })).ToList();

    public async Task<List<UserModel>> spGetByRoleName(string roleName) =>
        (await _db.LoadDataAsync<UserModel, dynamic>("spUser_GetByRoleName", new { roleName })).ToList();

    public async Task<List<UserModel>> GetUserByUserRoleIdAsync(int userRoleId) =>
        (await _db.LoadDataAsync<UserModel, dynamic>("spUser_GetByUserRoleId", new { userRoleId })).ToList();

    public async Task<User?> SaveUserAsync(UserModel user, List<UserApproverModel?> userApprovers, int userId)
    {
        await ValidateAsync(user);

        var _user = _mapper.Map<User>(user);

        if (_user.Id == 0)
        {
            _user = await CreateAsync(_user, userId);
        }
        else _user = await UpdateAsync(_user, userId);

        if (userApprovers != null)
        {
            var count = 1;
            foreach (var userApprover in userApprovers)
            {
                userApprover.Level = count;
                userApprover.UserId = _user.Id;
                var _userApprover = _mapper.Map<UserApprover>(userApprover);

                if (userApprover.Id == 0) await _userApproverRepo.CreateAsync(_userApprover, userId);
                else await _userApproverRepo.UpdateAsync(_userApprover, userId);

                count++;
            }

            var userApproverIds = userApprovers.Where(m => m.Id != 0).Select(m => m.Id).ToList();
            var toDelete = await _context.UserApprovers
                .Where(m => m.UserId == user.Id && !userApproverIds.Contains(m.Id))
                .Select(m => m.Id)
                .ToArrayAsync();

            await _userApproverRepo.BatchDeleteAsync(toDelete);
        }

        return _user;
    }

    public async Task<User> CreateAsync(User user, int userId)
    {
        user.CreatedById = userId;
        user.DateCreated = DateTime.Now;

        user = await _contextHelper.CreateAsync(user, "ModifiedById", "DateModified");

        return user;
    }

    public async Task<User> UpdateAsync(User user, int userId)
    {
        List<string> excludedPropertiesList = new List<string>
            {
                "LastFailedAttempt", "LockedTime", "FailedAttempts",
                "LastOnlineTime", "IsOnline", "CreatedById",
                "DateCreated", "Password", "PasswordSalt"
            };

        if (string.IsNullOrWhiteSpace(user.ProfilePicture))
        {
            excludedPropertiesList.Add("ProfilePicture");
        }

        if (string.IsNullOrWhiteSpace(user.Signature))
        {
            excludedPropertiesList.Add("Signature");
        }
        // Convert list to array
        string[] excludedProperties = excludedPropertiesList.ToArray();

        user.ModifiedById = userId;
        user.DateModified = DateTime.Now;

        user = await _contextHelper.UpdateAsync(user, excludedProperties);

        return user;
    }

    public async Task BatchDeleteAsync(int[] ids)
    {
        var entities = await _context.Users.Where(m => ids.Contains(m.Id)).ToListAsync();

        if (entities is not null)
            await _contextHelper.BatchDeleteAsync(entities);
    }

    private async Task ValidateAsync(UserModel model)
    {
        try
        {
            var users = await GetUsersAsync();

            if (model.Id == 0)
            {
                if (users.FirstOrDefault(m => m.UserName == model.UserName) != null) throw new Exception("User Name already taken!");
            }
            else
            {
                if (users.FirstOrDefault(m => m.Id != model.Id && m.UserName == model.UserName) != null) throw new Exception("User Name already taken!");
            }
        }
        catch (Exception) { throw; }
    }
}