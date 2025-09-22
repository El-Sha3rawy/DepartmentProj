using MediatR;

public record UpdateDepartmentCommand (UpdateCommand updateCommand) : IRequest <bool>;

public class UpdateCommand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Manager { get; set; }
}