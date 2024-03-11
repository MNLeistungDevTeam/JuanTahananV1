﻿using DMS.Domain.Dto.ApplicantsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.ApplicantsRepository
{
    public interface IApplicantsPersonalInformationRepository
    {
        Task<ApplicantsPersonalInformation?> GetByIdAsync(int id);

        Task<ApplicantsPersonalInformation?> GetbyUserId(int id);

        Task<List<ApplicantsPersonalInformation>?> GetAllAsync();

        Task DeleteAsync(int id);

        Task BatchDeleteAsync(int[] ids);

        Task<ApplicantsPersonalInformationModel?> GetByCodeAsync(string code);

        Task<ApplicantsPersonalInformationModel?> GetByUserAsync(int userId);

        Task<ApplicantsPersonalInformationModel?> GetAsync(int id);
        Task<ApplicantsPersonalInformation> SaveAsync(ApplicantsPersonalInformationModel model, int userId);
        Task<ApplicantsPersonalInformation> CreateAsync(ApplicantsPersonalInformation applicantPersonalInfo, int userId);
        Task<ApplicantsPersonalInformation> UpdateAsync(ApplicantsPersonalInformation applicantPersonalInfo, int userId);
    }
}