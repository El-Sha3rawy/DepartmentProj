using AutoMapper;
using DomainLayer;
using Shared;
using Shared.Proto;

namespace ApplicationLayer.DepartmentsCqrs.Behaviours
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<CreateDeptRequest, DepartmentDto>();
            CreateMap<UpdateDeptRequest, DepartmentDto>();
            CreateMap<DepartmentDto, DeptResponse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdated, opt => opt.Ignore());
        }
    }
}
