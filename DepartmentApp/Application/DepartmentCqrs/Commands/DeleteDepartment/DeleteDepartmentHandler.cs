using MediatR;
using System.ComponentModel.DataAnnotations;

public class DelteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, bool>
{
    private readonly IDepartmentRepository _repository;

    public DelteDepartmentHandler(IDepartmentRepository iropo) => _repository = iropo;

    public async Task <bool> Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
    {
        var check = await _repository.GetDepartmentById(command.id);
        if (check == null)
        {
            throw new ValidationException("The Department Is Not Existed");
        }
        return await _repository.DeleteDepartment(command.id);
    }
}