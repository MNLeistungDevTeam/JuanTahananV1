using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.Security.Resources;
using DevExpress.Utils;
using DevExpress.XtraReports.Web.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Serilog;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using Template.Application;
using DMS.Application.Services;
using Template.Infrastructure;
using DMS.Infrastructure.Hubs;
using DMS.Infrastructure.Persistence.Configuration;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using DMS.Infrastructure.Persistence;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("serilog.json")
        .Build())
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    builder.Services.AddControllers();

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
   // builder.Services.AddRazorPages();
    builder.Services.AddSignalR();
    builder.Services.AddHttpContextAccessor();

    // Application, Infrastructure Dependency Injection
    builder.Services.Configure<AuthenticationConfig>(builder.Configuration.GetSection("AuthenticationConfig"));
    builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("JWTConfiguration"));

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

   

    #region Hangfire

    builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

    builder.Services.AddHangfireServer();

    var hangFireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");
    builder.Services.AddDbContext<HangFireDBContext>(options =>
    {
        options.EnableSensitiveDataLogging();
        options.UseSqlServer(hangFireConnectionString);
    });

    #endregion Hangfire

    #region DevExpress

    builder.Services.AddDevExpressControls();
    string reportStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "EditedReports");

    // Register the custom report storage
    builder.Services.AddSingleton<ReportStorageWebExtension>(
        new CustomReportStorageWebExtension(reportStoragePath)
    );
    builder.Services.ConfigureReportingServices(configurator =>
    {
        configurator.ConfigureReportDesigner(designerConfigurator =>
        {
            designerConfigurator.RegisterDataSourceWizardConfigFileJsonConnectionStringsProvider();
            designerConfigurator.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
        });
        configurator.ConfigureWebDocumentViewer(viewerConfigurator =>
        {
            viewerConfigurator.UseCachedReportSourceBuilder();
        });
        configurator.UseAsyncEngine();
    });

    #endregion DevExpress

    #region Authentication

    string jwtKey = builder.Configuration["JWTConfiguration:SecretKey"];
    byte[] keyBytes = Encoding.ASCII.GetBytes(jwtKey ?? "");

    TokenValidationParameters tokenValidation = new()
    {
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };

    builder.Services.AddSingleton(tokenValidation);

    builder.Services.AddAuthentication(options =>
    {
        // custom scheme defined in .AddPolicyScheme() below
        options.DefaultScheme = "JWT_OR_COOKIE";
        options.DefaultChallengeScheme = "JWT_OR_COOKIE";
    })
         .AddCookie(options =>
         {
             options.Cookie.Name = "MNL_Template";
             //options.Events.OnRedirectToLogin = (context) =>
             //{
             //    context.Response.StatusCode = 401;
             //    return Task.CompletedTask;
             //};
         })
         .AddJwtBearer("Bearer", jwtOptions =>
          {
              jwtOptions.TokenValidationParameters = tokenValidation;
              jwtOptions.Events = new JwtBearerEvents
              {
                  OnTokenValidated = async (context) =>
                  {
                      IJwtService jwtService = builder.Services.BuildServiceProvider()
                                                               .CreateScope().ServiceProvider
                                                               .GetRequiredService<IJwtService>();
                      JwtSecurityToken jwtToken = context.SecurityToken as JwtSecurityToken;
                      bool isValid = await jwtService.IsTokenValid(jwtToken?.RawData ?? "");

                      if (!isValid)
                      {
                          context.Fail("Invalid Token Details");
                      }
                  }
              };
          })
        // this is the key piece!
        .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
        {
            // runs on each request
            options.ForwardDefaultSelector = context =>
            {
                // filter by auth type
                string authorization = (string)context.Request.Headers[HeaderNames.Authorization];
                if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                {
                    return "Bearer";
                }

                // otherwise always check for cookie auth
                return "Cookies";
            };
        });

    #endregion Authentication
 
    #region Localization

    //builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
    builder.Services.AddMvc()
        .AddViewLocalization()
        .AddDataAnnotationsLocalization();

    builder.Services.AddHttpContextAccessor();

    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("de-DE")
            // Add other supported cultures here
        };

        options.DefaultRequestCulture = new RequestCulture("en-US");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;

        options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
    });

    #endregion Localization

    #region CORS Policy

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAnyOrigin",
            policy =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
    });

    #endregion CORS Policy

    var app = builder.Build();


    #region Localization

    var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
    app.UseRequestLocalization(localizationOptions.Value);

    #endregion Localization


   

    #region DevExpress

    var contentDirectoryAllowRule = DirectoryAccessRule.Allow(new DirectoryInfo(Path.Combine(app.Environment.ContentRootPath, "..", "Content")).FullName);
    AccessSettings.ReportingSpecificResources.TrySetRules(contentDirectoryAllowRule, UrlAccessRule.Allow());
    DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Expressions;
    app.UseDevExpressControls();
    System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
    foreach (var assembly in assemblies)
    {
        var types = assembly.GetTypes();
        DeserializationSettings.RegisterTrustedAssembly(assembly);
    }
    #endregion DevExpress


    //Hangfire Authentication
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        //Authorization = new[]
        //    {
        //    new HangfireCustomBasicAuthenticationFilter
        //    {
        //        User = app.Configuration.GetSection("HangfireOptions:User").Value,
        //        Pass = app.Configuration.GetSection("HangfireOptions:Pass").Value
        //    }
        //}
    });

    if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
    {
        //app.UseDeveloperExceptionPage();
        app.UseExceptionHandler("/Home/Error");
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.Use(async (context, next) =>
    {
        await next();
        if (context.Response.StatusCode == 404)
        {
            context.Request.Path = "/Home/NotFoundPage";
            await next();
        }

        context.Request.Path = "/Home/BadRequestPage";
        //await next();
    });

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
 

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapHub<ChatHub>("/chatHub");
    app.MapHub<ProgressHub>("/progressHub");
    app.MapHub<AuthenticationHub>("/authenticationHub");
    app.MapHub<UploaderHub>("/uploaderHub");
    app.MapHub<NotificationHub>("/notificationHub");
    app.MapHub<OnlineUserHub>("/onlineUserHub");
    app.UseCors("AllowAnyOrigin");

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var db = services.GetRequiredService<HangFireDBContext>();
        db.InitializeDatabase();
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}