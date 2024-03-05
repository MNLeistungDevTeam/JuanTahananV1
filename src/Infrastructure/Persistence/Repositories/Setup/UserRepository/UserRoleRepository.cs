﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Interfaces.Setup.UserRepository;
using Template.Application.Services;
using Template.Domain.Dto.ModuleDto;
using Template.Domain.Dto.RoleDto;
using Template.Domain.Dto.UserDto;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence.Repositories.Setup.UserRepository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly MNLTemplateDBContext _context;
        private readonly EfCoreHelper<UserRole> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public UserRoleRepository(MNLTemplateDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<UserRole>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }
        public async Task<List<UserRoleModel>> SpGetAllRoles() =>
        (await _db.LoadDataAsync<UserRoleModel, dynamic>("spUserRole_GetAllRoles", new { })).ToList();
        public async Task<UserRole?> GetByIdAsync(int id) =>
        await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        public async Task<UserRole?> GetUserRoleAsync(int UserId) =>
            await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == UserId);
        public async Task<List<UserRole>?> GetAllAsync() =>
        await _context.UserRoles.AsNoTracking().ToListAsync();
        public async Task<UserRole> SaveAsync(UserRoleModel model)
        {
            if (model.Id == 0)
                model = _mapper.Map<UserRoleModel>(await CreateAsync(model));
            else
                model = _mapper.Map<UserRoleModel>(await UpdateAsync(model));
            return _mapper.Map<UserRole>(model);
        }
        public async Task<UserRole> CreateAsync(UserRoleModel model)
        {
            var mapped = _mapper.Map<UserRole>(model);
            mapped = await _contextHelper.CreateAsync(mapped);
            return mapped;
        }
        public async Task<UserRole> UpdateAsync(UserRoleModel model)
        {
            var mapped = _mapper.Map<UserRole>(model);
            mapped = await _contextHelper.UpdateAsync(mapped);
            return mapped;
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _contextHelper.GetByIdAsync(id);
            if (entity != null)
                await _contextHelper.DeleteAsync(entity);
        }
        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = await _context.UserRoles.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }

        }
    }
}
