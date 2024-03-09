using AutoMapper;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using DMS.Domain.Dto.ApplicantsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Infrastructure.Persistence;
using DMS.Infrastructure.PredefinedReports;
using DMS.Web.Controllers.Services;
using DMS.Web.Models;

namespace DMS.Web.Controllers;

[Authorize]
public class ReportController : Controller
{
    private readonly ReportDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IUserRepository _userRepo;
    private readonly IApplicantsPersonalInformationRepository _applicantsPersonalInformationRepo;
    private readonly ILoanParticularsInformationRepository _loanParticularsInformationRepo;
    private readonly ICollateralInformationRepository _collateralInformationRepo;
    private readonly IBarrowersInformationRepository _barrowersInformationRepo;
    private readonly ISpouseRepository _spouseRepo;
    private readonly IMapper _mapper;
    private readonly IForm2PageRepository _form2PageRepo;
    private DMSDBContext _tmpcontext;

    public ReportController(ReportDbContext context, IWebHostEnvironment hostingEnvironment, IUserRepository userRepo, IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo, ILoanParticularsInformationRepository loanParticularsInformationRepo, ICollateralInformationRepository collateralInformationRepo, IBarrowersInformationRepository barrowersInformationRepo, ISpouseRepository spouseRepo, IMapper mapper, IForm2PageRepository form2PageRepo, DMSDBContext tmpcontext)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _userRepo = userRepo;
        _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo;
        _loanParticularsInformationRepo = loanParticularsInformationRepo;
        _collateralInformationRepo = collateralInformationRepo;
        _barrowersInformationRepo = barrowersInformationRepo;
        _spouseRepo = spouseRepo;
        _mapper = mapper;
        _form2PageRepo = form2PageRepo;
        _tmpcontext = tmpcontext;
    }

    public List<ReportListViewModel> GetReportList()
    {
        List<ReportListViewModel> reportsList = new();
        List<ReportListViewModel> predefinedReports = ReportsFactory.Reports.Select(m => new ReportListViewModel { Name = m.Key, DisplayName = m.Key, IsCustomReport = false }).ToList();
        List<ReportListViewModel> savedReports = _context.Reports.Select(m => new ReportListViewModel { Name = m.Name, DisplayName = m.DisplayName, IsCustomReport = true }).ToList();

        reportsList.AddRange(predefinedReports);
        reportsList.AddRange(savedReports);

        return reportsList;
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteReportAsync(string reportName, CancellationToken cancellationToken)
    {
        var toDelete = _context.Reports.FirstOrDefault(m => m.Name == reportName);
        if (toDelete is null)
            return Conflict("Report not found!");

        _context.Remove(toDelete);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

        return Ok();
    }

    public async Task<IActionResult> Designer(
            [FromServices] IReportDesignerClientSideModelGenerator clientSideModelGenerator,
            [FromQuery] string reportName)
    {
        ReportDesignerCustomModel model = new ReportDesignerCustomModel();
        model.ReportDesignerModel = await CreateDefaultReportDesignerModel(clientSideModelGenerator, reportName, null);
        return View(model);
    }

    public static Dictionary<string, object> GetAvailableDataSources()
    {
        var dataSources = new Dictionary<string, object>();
        return dataSources;
    }

    public static async Task<ReportDesignerModel> CreateDefaultReportDesignerModel(IReportDesignerClientSideModelGenerator clientSideModelGenerator, string reportName, XtraReport report)
    {
        reportName = string.IsNullOrEmpty(reportName) ? "TestReport" : reportName;
        var dataSources = GetAvailableDataSources();
        if (report != null)
        {
            return await clientSideModelGenerator.GetModelAsync(report, dataSources, ReportDesignerController.DefaultUri, WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
        }
        return await clientSideModelGenerator.GetModelAsync(reportName, dataSources, ReportDesignerController.DefaultUri, WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
    }

    public async Task<IActionResult> Viewer(
        [FromServices] IWebDocumentViewerClientSideModelGenerator clientSideModelGenerator,
        [FromQuery] string reportName)
    {
        var reportToOpen = string.IsNullOrEmpty(reportName) ? "TestReport" : reportName;
        var model = new ViewerModel
        {
            ViewerModelToBind = await clientSideModelGenerator.GetModelAsync(reportToOpen, WebDocumentViewerController.DefaultUri)
        };
        return View(model);
    }

    public async Task<IActionResult> BCFFORM([FromServices] IWebDocumentViewerClientSideModelGenerator clientSideModelGenerator)
    {
        var model = new ViewerModel
        { 
            ViewerModelToBind = await clientSideModelGenerator.GetModelAsync("BuyerConfirmationForm", WebDocumentViewerController.DefaultUri)
        };
        model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters[0].Value = _hostingEnvironment.WebRootPath;
        model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters[1].Value = "dawda";
        return View("Viewer", model);
    }
    public async Task<IActionResult> LatestHousingForm([FromServices] IWebDocumentViewerClientSideModelGenerator clientSideModelGenerator,int userId)
    {
        var model = new ViewerModel
        {
            ViewerModelToBind = await clientSideModelGenerator.GetModelAsync("HousingForm", WebDocumentViewerController.DefaultUri)
        };
        var modelData = new ApplicantViewModel();
        if (userId != 0)
        {
            var latestApplicationForm = (await _applicantsPersonalInformationRepo
                .GetAllAsync())
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefault() ?? new ApplicantsPersonalInformation()
                {
                    UserId = userId
                };
            modelData.ApplicantsPersonalInformationModel = _mapper.Map<ApplicantsPersonalInformationModel>(latestApplicationForm);
            modelData.LoanParticularsInformationModel = _mapper.Map<LoanParticularsInformationModel>(await _loanParticularsInformationRepo.GetByApplicationIdAsync(latestApplicationForm.Id)) ?? new();
            modelData.CollateralInformationModel = _mapper.Map<CollateralInformationModel>(await _collateralInformationRepo.GetByApplicationInfoIdAsync(latestApplicationForm.Id)) ?? new();
            modelData.BarrowersInformationModel = _mapper.Map<BarrowersInformationModel>(await _barrowersInformationRepo.GetByApplicationInfoIdAsync(latestApplicationForm.Id)) ?? new();
            modelData.SpouseModel = _mapper.Map<SpouseModel>(await _spouseRepo.GetByApplicationInfoIdAsync(latestApplicationForm.Id)) ?? new();
            modelData.Form2PageModel = _mapper.Map<Form2PageModel>(await _form2PageRepo.GetByApplicationInfoIdAsync(latestApplicationForm.Id)) ?? new();
        }
        model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters[0].Value = _hostingEnvironment.WebRootPath;
        model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters[1].Value = JsonConvert.SerializeObject(modelData,Newtonsoft.Json.Formatting.Indented);
        return View("Viewer", model);
    }
    public async Task<IActionResult> HousingForm([FromServices] IWebDocumentViewerClientSideModelGenerator clientSideModelGenerator, int id)
    {
        var model = new ViewerModel
        {
            ViewerModelToBind = await clientSideModelGenerator.GetModelAsync("HousingForm", WebDocumentViewerController.DefaultUri)
        };
        model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters[0].Value = _hostingEnvironment.WebRootPath;
        model.ViewerModelToBind.ReportInfo.ParametersInfo.Parameters[1].Value = "dawda";
        return View("Viewer", model);
    }
}