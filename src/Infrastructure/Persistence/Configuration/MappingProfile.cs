using AutoMapper;
using Template.Domain.Dto.ApplicantsDto;
using Template.Domain.Dto.DocumentDto;
using Template.Domain.Dto.ModuleDto;
using Template.Domain.Dto.RoleDto;
using Template.Domain.Dto.UserDto;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence.Configuration;

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
        CreateMap<ApplicantsPersonalInformation, ApplicantsPersonalInformationModel>().ReverseMap();
        CreateMap<LoanParticularsInformation, LoanParticularsInformationModel>().ReverseMap();
        CreateMap<CollateralInformation, CollateralInformationModel>().ReverseMap();
        CreateMap<BarrowersInformation, BarrowersInformationModel>().ReverseMap();
        CreateMap<Spouse, SpouseModel>().ReverseMap();
        CreateMap<UserDocument, UserDocumentModel>().ReverseMap();
        CreateMap<Form2Page, Form2PageModel>().ReverseMap();
    }
}