using DMS.Application.Interfaces.Setup.UserCompanyRepo;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.UserCompanyRepo
{
    public class UserCompanyRepository : IUserCompanyRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<UserCompany> _contextHelper;

        public UserCompanyRepository(DMSDBContext context)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<UserCompany>(context);
        }


        public async Task<UserCompany?> GetByIdAsync(int id)
        {
            try
            {
                var data = await _contextHelper.GetByIdAsync(id);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserCompany>> GetByUserIdAsync(int userId)
        {
            try
            {
                var data = await _context.UserCompanies.Where(m => m.UserId == userId).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserCompany>> GetAllAsync()
        {
            try
            {
                var data = await _contextHelper.GetAllAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task SaveAsync(int? developerId, int userId, int editorId)
        {
            try
            {
                // No selected developer, delete existing UserCompany entries
                if (!developerId.HasValue)
                {
                    var existingCompanyIds = await _context.UserCompanies
                              .Where(m => m.UserId == userId)
                              .Select(m => m.Id)
                              .ToArrayAsync();

                    await BatchDeleteAsync(existingCompanyIds);
                }
                else
                {
                    var userCompany = await BuildUserCompanyData(developerId.Value, userId, editorId);

                    if (userCompany == null)
                        return;

                    UserCompany _userCompany;
                    if (userCompany.Id == 0)
                    {
                        _userCompany = await CreateAsync(userCompany, userId);
                    }
                    else
                    {
                        _userCompany = await UpdateAsync(userCompany, userId);
                    }

                    var userCompanyId = _userCompany.Id;
                    var toDelete = await _context.UserCompanies
                        .Where(m => m.UserId == userId && m.Id != userCompanyId)
                        .Select(m => m.Id)
                        .ToArrayAsync();

                    await BatchDeleteAsync(toDelete);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<UserCompany> SaveAsync(UserCompany userProject, int userId)
        {
            try
            {
                if (userProject.Id == 0)
                {
                    userProject = await CreateAsync(userProject, userId);
                }
                else
                {
                    userProject = await UpdateAsync(userProject, userId);
                }

                return userProject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserCompany> CreateAsync(UserCompany userProject, int userId)
        {
            try
            {
                userProject.CreatedById = userId;
                userProject.DateCreated = DateTime.Now;

                userProject = await _contextHelper.CreateAsync(userProject, "ModifiedById", "DateModified");

                return userProject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserCompany> UpdateAsync(UserCompany userProject, int userId)
        {
            try
            {
                userProject.ModifiedById = userId;
                userProject.DateModified = DateTime.Now;

                userProject = await _contextHelper.UpdateAsync(userProject, "CreatedById", "DateCreated");

                return userProject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = await _context.UserCompanies.Where(m => ids.Contains(m.Id)).ToListAsync();

            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }

        private async Task<UserCompany> BuildUserCompanyData(int developerId, int userId, int editorId)
        {
            try
            {
                var userCompany = await _context.UserCompanies
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.UserId == userId && m.CompanyId == developerId);

                if (userCompany is not null)
                {
                    userCompany.DateModified = DateTime.Now;
                    userCompany.ModifiedById = editorId;
                }
                else
                {
                    userCompany = new UserCompany
                    {
                        Id = 0,
                        UserId = userId,
                        CompanyId = developerId,
                        DateCreated = DateTime.Now,
                        CreatedById = editorId
                    };
                }

                return userCompany;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}