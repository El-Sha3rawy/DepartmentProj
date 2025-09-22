
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, bool>
{
    private readonly IDepartmentRepository _repository;
    private readonly IMapper _mapper ;

    public CreateDepartmentHandler(IMapper mapper , IDepartmentRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task <bool> Handle(CreateDepartmentCommand command,CancellationToken cancellationToken)
    {
        var check = await _repository.GetDepartmentByName(0,command.createCommand.Name); 
        
        if (check)
        {
            throw new ValidationException("This Name is Already Existed");
        }

        var department = _mapper.Map<Department> (command.createCommand);
        department.CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        await _repository.CreateDepartment(department);
        return true;
    }


}
