
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

public class GetDepartmentHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
{
    private readonly IDepartmentRepository _repository;
    private readonly IMapper _mapper; 

    public GetDepartmentHandler(IMapper mapper,IDepartmentRepository repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task <DepartmentDto> Handle (GetDepartmentByIdQuery query , CancellationToken cancellationToken)
    {
        var Dept = await _repository.GetDepartmentById(query.id);

        if (Dept == null)
        {
            throw new ValidationException("This Department Is Not Found");
        }
        return _mapper.Map<DepartmentDto>(Dept);
        
    }
}