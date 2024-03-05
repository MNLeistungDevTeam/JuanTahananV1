﻿using DMS.Domain.Dto.ApplicantsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.ApplicantsRepository
{
    public interface ILoanParticularsInformationRepository
    {
        Task<LoanParticularsInformation?> GetByIdAsync(int id);
        Task<LoanParticularsInformation?> GetByApplicationIdAsync(int id);
        Task<LoanParticularsInformation> SaveAsync(LoanParticularsInformationModel model);
        Task<LoanParticularsInformation> CreateAsync(LoanParticularsInformationModel model);
        Task<LoanParticularsInformation> UpdateAsync(LoanParticularsInformationModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
