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

        Task DeleteAsync(int id);

        Task BatchDeleteAsync(int[] ids);

        Task<LoanParticularsInformationModel?> GetByApplicantIdAsync(int applicantId);
        Task<LoanParticularsInformation> SaveAsync(LoanParticularsInformationModel model, int userId);
        Task<LoanParticularsInformation> CreateAsync(LoanParticularsInformation loanParticulars, int userId);
        Task<LoanParticularsInformation> UpdateAsync(LoanParticularsInformation loanParticulars, int userId);
    }
}