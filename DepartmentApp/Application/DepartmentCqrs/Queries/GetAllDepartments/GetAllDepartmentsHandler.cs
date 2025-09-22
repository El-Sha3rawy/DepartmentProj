
using AutoMapper;
using MediatR;

public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentDto>>
{
    private readonly IDepartmentRepository _repository;
    private readonly IMapper _mapper;

    public GetAllDepartmentsHandler (IDepartmentRepository repository , IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentDto>> Handle(GetAllDepartmentsQuery query,CancellationToken cancellationToken)
    {
        IEnumerable<Department> Depts = await _repository.GetAllDepartments();

        return _mapper.Map<IEnumerable<DepartmentDto>>(Depts);
    }
}