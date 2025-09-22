
using AutoMapper;

public class DepartmentMapping : Profile
{
    public DepartmentMapping()
    {
        CreateMap<Department,DepartmentDto>().ReverseMap();
        CreateMap<Department,CreateCommand>().ReverseMap();
        CreateMap<Department,UpdateCommand>().ReverseMap();
    }
}