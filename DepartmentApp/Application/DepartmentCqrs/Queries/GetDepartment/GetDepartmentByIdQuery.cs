using MediatR;

public record GetDepartmentByIdQuery (int id) : IRequest<DepartmentDto>;
    
    
