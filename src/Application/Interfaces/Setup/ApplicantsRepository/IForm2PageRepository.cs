﻿using DMS.Domain.Dto.ApplicantsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.ApplicantsRepository
{
    public interface IForm2PageRepository
    {
        Task<Form2Page?> GetByIdAsync(int id);
        Task<Form2Page?> GetByApplicationInfoIdAsync(int id);
        Task<Form2Page> SaveAsync(Form2PageModel model);
        Task<Form2Page> CreateAsync(Form2PageModel model);
        Task<Form2Page> UpdateAsync(Form2PageModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
        Task<Form2PageModel?> GetByApplicantIdAsync(int applicantId);
    }
}
