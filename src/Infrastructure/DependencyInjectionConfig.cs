using AutoMapper;
using DMS.Domain.Dto.EmailSettingsDto;
using DMS.Domain.Dto.FtpServerConfigDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DMS.Application.Interfaces.Setup;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.DocumentRepository;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Infrastructure.Persistence;
using DMS.Infrastructure.Persistence.Configuration;
using DMS.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository;
using DMS.Infrastructure.Persistence.Repositories.Setup.DocumentRepository;
using DMS.Infrastructure.Persistence.Repositories.Setup.ModuleRepository;
using DMS.Infrastructure.Persistence.Repositories.Setup.RoleRepository;
using DMS.Infrastructure.Persistence.Repositories.Setup.UserRepository;
using DMS.Infrastructure.Services;
using DMS.Application.Interfaces.Setup.CompanyRepo;
using DMS.Infrastructure.Persistence.Repositories.Setup.CompanyRepo;
using DMS.Application.Interfaces.Setup.CompanySettingRepo;
using DMS.Application.Interfaces.Setup.CompanyLogoRepo;
using DMS.Infrastructure.Persistence.Repositories.Setup.CompanyLogoRepo;
using DMS.Domain.Entities;
using DMS.Infrastructure.Persistence.Repositories.Setup.CompanySettingRepo;
using DMS.Infrastructure.Persistence.Repositories.Setup.AddressRepo;
using DMS.Application.Interfaces.Setup.AddressRepo;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DMS.Infrastructure.Persistence.Repositories.Setup.CountryRepo;
using DMS.Application.Interfaces.Setup.CountryRepo;
using DMS.Application.Interfaces.Setup.AddressTypeRepo;
using DMS.Infrastructure.Persistence.Repositories.Setup.AddressTypeRepo;
using DMS.Application.Interfaces.Setup.PurposeOfLoanRepo;
using DMS.Infrastructure.Persistence.Repositories.Setup.PurposeOfLoanRepo;
using DMS.Application.Interfaces.Setup.ModeOfPaymentRepo;
using DMS.Application.Interfaces.Setup.PropertyTypeRepo;
using DMS.Infrastructure.Persistence.Repositories.Setup.PropertyTypeRepo;
using DMS.Infrastructure.Persistence.Repositories.Setup.ModeOfPaymentRepo;
using DMS.Application.Interfaces.Setup.NotificationReceiverRepo;
using DMS.Infrastructure.Persistence.Repositories.Setup.NotificationReceiverRepo;
using DMS.Application.Interfaces.Setup.NotificationRepo;
using DMS.Infrastructure.Persistence.Repositories.Setup.NotificationRepo;

namespace Template.Infrastructure;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConstr = configuration.GetConnectionString("Default");

        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddDbContext<ReportDbContext>(options =>
        {
            options.UseSqlServer(defaultConstr);
        });

        services.AddDbContext<DMSDBContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(defaultConstr);
        });

        services.AddScoped<IReportDbContext>(provider => provider.GetRequiredService<ReportDbContext>());
        //services.AddScoped<IDMSDBContext>(provider => provider.GetRequiredService<DMSDBContext>());
        services.Configure<FtpServerConfigModel>(options =>
        {
            options.FtpUser = configuration["FtpServer:FtpUser"];
            options.FtpPass = configuration["FtpServer:FtpPass"];
            options.FtpHost = configuration["FtpServer:FtpHost"];
        });
        services.Configure<EmailSettingsModel>(options =>
        {
            options.Email = configuration["EmailSettings:Email"];
            options.Password = configuration["EmailSettings:Password"];
            options.Host = configuration["EmailSettings:Host"];
            options.Displayname = configuration["EmailSettings:Displayname"];
            options.Port = int.Parse(configuration["EmailSettings:Port"] ?? "");
        });
        services.AddBrowserDetection();
        services.AddTransient<HttpClient>();
        services.AddSingleton<ISQLDatabaseService, SQLDatabaseService>();
        services.AddScoped<IFtpDownloaderService, FtpDownloaderService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<ILocalizationService, LocalizationService>();
        services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleAccessRepository, RoleAccessRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserApproverRepository, UserApproverRepository>();
        services.AddScoped<IUserActivityRepository, UserActivityRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ILongOperationSignalRService, LongOperationSignalRService>();
        services.AddScoped<IApplicantsPersonalInformationRepository, ApplicantsPersonalInformationRepository>();
        services.AddScoped<ICollateralInformationRepository, CollateralInformationRepository>();
        services.AddScoped<IBarrowersInformationRepository, BarrowersInformationRepository>();
        services.AddScoped<ILoanParticularsInformationRepository, LoanParticularsInformationRepository>();
        services.AddScoped<ISpouseRepository, SpouseRepository>();
        services.AddScoped<IUserDocumentRepository, UserDocumentRepository>();
        services.AddScoped<IForm2PageRepository, Form2PageRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanySettingRepository, CompanySettingRepository>();
        services.AddScoped<ICompanyLogoRepository, CompanyLogoRepository>();

        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IAddressTypeRepository, AddressTypeRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();

        services.AddScoped<IPurposeOfLoanRepository, PurposeOfLoanRepository>();
        services.AddScoped<IModeOfPaymentRepository, ModeOfPaymentRepository>();
        services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();

        services.AddScoped<INotificationReceiverRepository, NotificationReceiverRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }
}