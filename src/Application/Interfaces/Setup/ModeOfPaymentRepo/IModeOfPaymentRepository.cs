using DMS.Domain.Dto.ModeOfPaymentDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.ModeOfPaymentRepo
{
    public interface IModeOfPaymentRepository
    {
        Task<IEnumerable<ModeOfPaymentModel>> GetAllAsync();
        Task<ModeOfPaymentModel?> GetAsync(int id);
        Task<ModeOfPayment> SaveAsync(ModeOfPaymentModel model);
    }
}
