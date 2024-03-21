using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.SourcePagibigFundRepo
{
    public interface ISourcePagibigFundRepository
    {
        Task<List<SourcePagibigFund>?> GetAllAsync();
    }
}