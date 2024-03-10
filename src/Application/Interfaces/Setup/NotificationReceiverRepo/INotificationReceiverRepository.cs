using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.NotificationReceiverRepo
{
    public interface INotificationReceiverRepository
    {
        Task BatchDeleteAsync(int[] ids);

        Task DeleteAsync(int id);

        Task<NotificationReceiver> SaveAsync(NotificationReceiver notifreceiver, int userId);

        Task<NotificationReceiver> UpdateAsync(NotificationReceiver notifreceiver, int userId);
    }
}