using AutoMapper;
using DMS.Application.Interfaces.Setup.NotificationReceiverRepo;
using DMS.Application.Services;
using DMS.Domain.Entities;
using System;
using System.Linq;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.NotificationReceiverRepo
{
    public class NotificationReceiverRepository : INotificationReceiverRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<NotificationReceiver> _contextHelper;
        private readonly ISQLDatabaseService _db;
        private readonly IMapper _mapper;

        public NotificationReceiverRepository(DMSDBContext context, ISQLDatabaseService db, IMapper mapper)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<NotificationReceiver>(context);
            _db = db;
            _mapper = mapper;
        }

        public async Task<NotificationReceiver> SaveAsync(NotificationReceiver notifreceiver, int userId)
        {
            if (notifreceiver.Id == 0)
                notifreceiver = await CreateAsync(notifreceiver, userId);
            else
                notifreceiver = await UpdateAsync(notifreceiver, userId);

            return notifreceiver;
        }

        public async Task<NotificationReceiver> CreateAsync(NotificationReceiver notifreceiver, int userId)
        {
            var result = await _contextHelper.CreateAsync(notifreceiver);

            return result;
        }

        public async Task<NotificationReceiver> UpdateAsync(NotificationReceiver notifreceiver, int userId)
        {
            notifreceiver = await _contextHelper.UpdateAsync(notifreceiver);

            return notifreceiver;
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.NotificationReceivers.Where(v => ids.Contains(v.Id));

            await _contextHelper.BatchDeleteAsync(entities);
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.NotificationReceivers.FirstOrDefault(v => id == v.Id);

            await _contextHelper.DeleteAsync(entities);
        }
    }
}