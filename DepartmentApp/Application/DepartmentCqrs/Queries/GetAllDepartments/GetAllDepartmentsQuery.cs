using MediatR;

public record GetAllDepartmentsQuery : IRequest<IEnumerable<DepartmentDto>>;
    
    

