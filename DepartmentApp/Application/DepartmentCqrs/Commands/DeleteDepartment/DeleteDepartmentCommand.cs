using MediatR;

public record DeleteDepartmentCommand (int id) : IRequest<bool>;