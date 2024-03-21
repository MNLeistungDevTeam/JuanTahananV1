using AutoMapper;
using DMS.Application.Interfaces.Setup.SourcePagibigFundRepo;
using DMS.Application.Services;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.SourcePagibigFundRepo
{
    public class SourcePagibigFundRepository : ISourcePagibigFundRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<SourcePagibigFund> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public SourcePagibigFundRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<SourcePagibigFund>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<List<SourcePagibigFund>?> GetAllAsync() =>
                await _context.SourcePagibigFunds.AsNoTracking().ToListAsync();
    }
}