using DMS.Domain.Dto.TemporaryLinkDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.TemporaryLinkRepo
{
    public interface ITemporaryLinkRepository
    {
        Task<TemporaryLink>? GetTemporaryLinkData(TemporaryLinkModel model);
        Task<TemporaryLink> SaveAsync(TemporaryLinkModel model);
        Task<TemporaryLink> SaveContextAsync(TemporaryLink model);
    }
}
