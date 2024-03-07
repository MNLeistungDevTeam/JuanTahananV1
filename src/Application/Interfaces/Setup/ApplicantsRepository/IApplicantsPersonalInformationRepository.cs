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
        Task<ApplicantsPersonalInformation> SaveAsync(ApplicantsPersonalInformationModel model);
        Task<ApplicantsPersonalInformation> CreateAsync(ApplicantsPersonalInformation model);
        Task<ApplicantsPersonalInformation> UpdateAsync(ApplicantsPersonalInformation model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
