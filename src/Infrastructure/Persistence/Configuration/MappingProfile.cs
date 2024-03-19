using AutoMapper;
using DMS.Application.Interfaces.Setup.NotificationReceiverRepo;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.ApprovalLevelDto;
using DMS.Domain.Dto.ApprovalLogDto;
using DMS.Domain.Dto.ApprovalStatusDto;
using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Dto.DocumentDto;
using DMS.Domain.Dto.EntityDto;
using DMS.Domain.Dto.ModuleDto;
using DMS.Domain.Dto.ModuleStageApproverDto;
using DMS.Domain.Dto.ModuleStageDto;
using DMS.Domain.Dto.ModuleTypeDto;
using DMS.Domain.Dto.NotificationDto;
using DMS.Domain.Dto.NotificationReceiverDto;
using DMS.Domain.Dto.RoleDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Add as many of these lines as you need to map your objects
        CreateMap<User, UserModel>().ReverseMap();
        CreateMap<Module, ModuleModel>().ReverseMap();
        CreateMap<Role, RoleModel>().ReverseMap();
        CreateMap<RoleAccess, RoleAccessModel>().ReverseMap();
        CreateMap<UserRole, UserRoleModel>().ReverseMap();
        CreateMap<DocumentType, DocumentTypeModel>().ReverseMap();
        CreateMap<Document, DocumentModel>().ReverseMap();
        CreateMap<ApplicantsPersonalInformation, ApplicantsPersonalInformationModel>().ReverseMap();
        CreateMap<LoanParticularsInformation, LoanParticularsInformationModel>().ReverseMap();
        CreateMap<CollateralInformation, CollateralInformationModel>().ReverseMap();
        CreateMap<BarrowersInformation, BarrowersInformationModel>().ReverseMap();
        CreateMap<Spouse, SpouseModel>().ReverseMap();
        CreateMap<UserDocument, UserDocumentModel>().ReverseMap();
        CreateMap<Form2Page, Form2PageModel>().ReverseMap();

        CreateMap<Company, CompanyModel>().ReverseMap();
        CreateMap<CompanySetting, CompanySettingModel>().ReverseMap();
        CreateMap<CompanyLogo, CompanyLogoModel>().ReverseMap();

        CreateMap<Address, AddressModel>().ReverseMap();
        CreateMap<AddressType, AddressTypeModel>().ReverseMap();
        CreateMap<Country, CountryModel>().ReverseMap();

        CreateMap<NotificationReceiver, NotificationReceiverModel>().ReverseMap();
        CreateMap<Notification, NotificationModel>().ReverseMap();

        CreateMap<Module, ModuleModel>().ReverseMap();
        CreateMap<ModuleType, ModuleTypeModel>().ReverseMap();
        CreateMap<ModuleStage, ModuleStageModel>().ReverseMap();
        CreateMap<ModuleStageApprover, ModuleStageApproverModel>().ReverseMap();



        CreateMap<ApprovalStatus, ApprovalStatusModel>().ReverseMap();
        CreateMap<ApprovalLevel, ApprovalLevelModel>().ReverseMap();
        CreateMap<ApprovalLog, ApprovalLogModel>().ReverseMap();
    }
}