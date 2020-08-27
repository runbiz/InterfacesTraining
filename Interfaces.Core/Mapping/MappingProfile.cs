using AutoMapper;
using Interfaces.Core.DTO;
using Interfaces.Entities;

namespace Interfaces.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Developer, DeveloperDto>();
            CreateMap<DeveloperDto, Developer>()
                .ForMember(m => m.Accounts, opt => opt.Ignore())
                .ForMember(m => m.Department, opt => opt.Ignore())
                .ForMember(m => m.DepartmentId, opt => opt.MapFrom(x => x.Department.Id));
            CreateMap<DeveloperForCreationDto, Developer>();
            CreateMap<DeveloperForUpdateDto, Developer>();

            CreateMap<Account, AccountDto>();

            CreateMap<Department, DepartmentDto>();
        }
    }
}
