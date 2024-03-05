using Microsoft.EntityFrameworkCore;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.UserRepository;

public class UserApproverRepository : IUserApproverRepository
{
    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<UserApprover> _contextHelper;

    public UserApproverRepository(DMSDBContext context)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<UserApprover>(context);
    }

    public async Task<UserApprover> CreateAsync(UserApprover userApprover, int userId)
    {
        string[] toIgnore = new[] { "ModifiedById", "DateModified" };
        userApprover.CreatedById = userId;
        userApprover.DateCreated = DateTime.UtcNow;

        userApprover = await _contextHelper.CreateAsync(userApprover, toIgnore);
        return userApprover;
    }

    public async Task<UserApprover> UpdateAsync(UserApprover userApprover, int userId)
    {
        userApprover.ModifiedById = userId;
        userApprover.DateModified = DateTime.UtcNow;

        userApprover = await _contextHelper.UpdateAsync(userApprover, "CreatedById", "DateCreated");
        return userApprover;
    }

    public async Task BatchDeleteAsync(int[] ids)
    {
        var entities = await _context.UserApprovers.Where(m => ids.Contains(m.Id)).ToListAsync();

        if (entities is not null)
            await _contextHelper.BatchDeleteAsync(entities);
    }
}