
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

public class UpdateDepartmentHandler : IRequestHandler< UpdateDepartmentCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly IDepartmentRepository _repository;

    public UpdateDepartmentHandler(IMapper mapper , IDepartmentRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task <bool> Handle( UpdateDepartmentCommand command , CancellationToken cancellation)
    {
        var Dept = await _repository.GetDepartmentById(command.updateCommand.Id);
        if (Dept == null)
        {
            throw new ValidationException("The Department Is not Exicted");
        }
        
        var Check = await _repository.GetDepartmentByName(command.updateCommand.Id, command.updateCommand.Name);
        if (Check)
        {
            throw new ValidationException("This Name Is Already Existed");
        }

        var update = _mapper.Map<Department>(command.updateCommand);
        await _repository.UpdateDepartment(update);
        
        return true;
    }

}
