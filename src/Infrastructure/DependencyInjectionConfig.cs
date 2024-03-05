using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Template.Application.Interfaces.Setup;
using Template.Application.Interfaces.Setup.ApplicantsRepository;
using Template.Application.Interfaces.Setup.DocumentRepository;
using Template.Application.Interfaces.Setup.ModuleRepository;
using Template.Application.Interfaces.Setup.RoleRepository;
using Template.Application.Interfaces.Setup.UserRepository;
using Template.Application.Services;
using Template.Domain.Dto.EmailSettingsDto;
using Template.Domain.Dto.FtpServerConfigDto;
using Template.Infrastructure.Persistence;
using Template.Infrastructure.Persistence.Configuration;
using Template.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository;
using Template.Infrastructure.Persistence.Repositories.Setup.DocumentRepository;
using Template.Infrastructure.Persistence.Repositories.Setup.ModuleRepository;
using Template.Infrastructure.Persistence.Repositories.Setup.RoleRepository;
using Template.Infrastructure.Persistence.Repositories.Setup.UserRepository;
using Template.Infrastructure.Services;

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

        services.AddDbContext<MNLTemplateDBContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(defaultConstr);
        });

        services.AddScoped<IReportDbContext>(provider => provider.GetRequiredService<ReportDbContext>());
        //services.AddScoped<IMNLTemplateDBContext>(provider => provider.GetRequiredService<MNLTemplateDBContext>());
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
        services.AddScoped<IRoleAccessRepository,RoleAccessRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserApproverRepository, UserApproverRepository>();
        services.AddScoped<IUserActivityRepository, UserActivityRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ILongOperationSignalRService, LongOperationSignalRService>();
        services.AddScoped<IApplicantsPersonalInformationRepository,ApplicantsPersonalInformationRepository>();
        services.AddScoped<ICollateralInformationRepository,CollateralInformationRepository>();
        services.AddScoped<IBarrowersInformationRepository,BarrowersInformationRepository>();
        services.AddScoped<ILoanParticularsInformationRepository, LoanParticularsInformationRepository>();
        services.AddScoped<ISpouseRepository, SpouseRepository>();
        services.AddScoped<IUserDocumentRepository, UserDocumentRepository>();
        services.AddScoped<IForm2PageRepository,Form2PageRepository>();
        return services;
    }
}