using AutoMapper;
using DMS.Application.Interfaces.Setup.AddressRepo;
using DMS.Application.Interfaces.Setup.CompanyLogoRepo;
using DMS.Application.Interfaces.Setup.CompanyRepo;
using DMS.Application.Interfaces.Setup.CompanySettingRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Dto.EntityDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.CompanyRepo;

public class CompanyRepository : ICompanyRepository
{
    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<Company> _contextHelper;
    private readonly ICompanySettingRepository _settingsRepository;
    private readonly ICompanyLogoRepository _logoRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;
    private readonly ISQLDatabaseService _db;

    public CompanyRepository(
        DMSDBContext context,
        IMapper mapper,
        ISQLDatabaseService db,
        ICompanySettingRepository settingsRepository,
        ICompanyLogoRepository logoRepository,
        IAddressRepository addressRepository)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<Company>(_context);
        _mapper = mapper;
        _db = db;
        _settingsRepository = settingsRepository;
        _logoRepository = logoRepository;
        _addressRepository = addressRepository;
    }

    #region Getter

    public async Task<Company?> GetByIdAsync(int id)
    {
        try
        {
            var data = await _contextHelper.GetByIdAsync(id);
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Company>> GetAllAsync()
    {
        try
        {
            var data = await _contextHelper.GetAllAsync();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<CompanyModel?> GetCompany(int id)
    {
        try
        {
            var data = await _db.LoadSingleAsync<CompanyModel, dynamic>("spCompany_Get", new { id });
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<CompanyModel>> GetCompanies()
    {
        try
        {
            var data = await _db.LoadDataAsync<CompanyModel, dynamic>("spCompany_GetAll", new { });
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<CompanyInfoModel?> GetCompanyInfo(int comapanyId)
    {
        try
        {
            var data = await _db.LoadSingleAsync<CompanyInfoModel, dynamic>("spCompany_GetInfo", new { comapanyId });
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Company?> GetCompanyByCode(string code)
    {
        try
        {
            var data = await _context.Companies.FirstOrDefaultAsync(m => m.Code == code);
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion Getter

    #region Operation

    public async Task SaveAsync(
        CompanyModel cModel,
        List<CompanyLogoModel> clModel,
        List<AddressModel> aModel,
        CompanySettingModel csModel,
        int userId,
        string webHostingEnv)
    {
        try
        {
            var _cModel = _mapper.Map<Company>(cModel);

            if (cModel == null)
                return;

            await ValidateCompany(_cModel);

            if (_cModel.Id == 0)
                _cModel = await CreateAsync(_cModel, userId);
            else
                _cModel = await UpdateAsync(_cModel, userId);

            // Company Settings
            var _csModel = _mapper.Map<CompanySetting>(csModel);
            _csModel.CompanyId = _cModel.Id;

            await _settingsRepository.SaveAsync(_csModel, userId);

            // Company Logo
            if (clModel is not null && clModel.Any())
            {
                var clList = new List<CompanyLogo>();
                foreach (var _loanPurposeLogo in clModel)
                {
                    var _clModel = _mapper.Map<CompanyLogo>(_loanPurposeLogo);
                    _clModel.CompanyId = _cModel.Id;

                    if (!string.IsNullOrEmpty(_clModel.Location))
                        _clModel = await _logoRepository.SaveAsync(_clModel, userId);
                    clList.Add(_clModel);
                }

                var logoIds = clList.Where(m => m.Id != 0).Select(m => m.Id).ToList();  // Remove the condition Id != 0

                var logosToDelete = await _context.CompanyLogos
                           .Where(m => m.CompanyId == _cModel.Id && !logoIds.Contains(m.Id))
                           .ToListAsync();

                await _logoRepository.BatchDeleteAsync(logosToDelete);

                DeleteProfilePictures(_cModel.Id, logosToDelete.ToList(), webHostingEnv);
            }

            // Address
            if (aModel is not null && aModel.Any())
            {
                var addressList = new List<Address>();
                foreach (var _address in aModel)
                {
                    _address.ReferenceId = _cModel.Id;
                    _address.ReferenceType = (int)AddressReferenceType.Index.Company;

                    var address = await _addressRepository.SaveAsync(_address, userId);

                    addressList.Add(address);
                }

                var addressIds = addressList.Where(m => m.Id != 0).Select(m => m.Id).ToList();  // Remove the condition Id != 0

                if (addressIds.Any())
                {
                    var addressToDelete = await _context.Addresses
                        .Where(m => m.ReferenceId == _cModel.Id && m.ReferenceType == (int)AddressReferenceType.Index.Company)
                        .Where(m => !addressIds.Contains(m.Id))
                        .ToListAsync();

                    await _addressRepository.BatchDeleteAsync(addressToDelete);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Company> CreateAsync(Company loanPurpose, int createdById)
    {
        try
        {
            loanPurpose.CreatedById = createdById;
            loanPurpose.DateCreated = DateTime.UtcNow;
            var result = await _contextHelper.CreateAsync(loanPurpose, "ModifiedById", "DateModified");

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Company> UpdateAsync(Company loanPurpose, int modifiedById)
    {
        try
        {
            loanPurpose.ModifiedById = modifiedById;
            loanPurpose.DateModified = DateTime.UtcNow;
            var result = await _contextHelper.UpdateAsync(loanPurpose, "CreatedById", "DateCreated");

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var entities = _context.Companies.FirstOrDefault(d => d.Id == id);
            if (entities is not null)
                await _contextHelper.DeleteAsync(entities);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task BatchDeleteAsync(int[] ids)
    {
        try
        {
            var entities = _context.Companies.Where(d => ids.Contains(d.Id));
            if (entities is not null)
                await _contextHelper.BatchDeleteAsync(entities);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async void DeleteProfilePictures(int loanPurposeId, List<CompanyLogo> logoList, string webHostingEnv)
    {
        try
        {
            var loanPurpose = await GetByIdAsync(loanPurposeId);

            if (loanPurpose == null)
                return;

            foreach (var item in logoList)
            {
                if (item == null)
                    return;

                string filePath = string.Format("{0}{1}", webHostingEnv, item.Location.Replace("/", "\\"));

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion Operation

    #region Validate Methods

    private async Task ValidateCompany(Company cModel)
    {
        try
        {
            var companies = await GetCompanies();

            if (companies.FirstOrDefault(m => m.Id != cModel.Id && m.Name == cModel.Name) != null)
                throw new Exception("Company Name already exists!");

            if (companies.FirstOrDefault(m => m.Id != cModel.Id && m.Code == cModel.Code) != null)
                throw new Exception("Company Code already exists!");
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion Validate Methods
}