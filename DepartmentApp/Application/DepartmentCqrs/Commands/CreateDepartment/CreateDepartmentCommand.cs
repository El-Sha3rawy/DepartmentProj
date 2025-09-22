using MediatR;

    public record CreateDepartmentCommand(CreateCommand createCommand): IRequest <bool>;
    
    public class CreateCommand
    {
        public string Name { get; set; }
        public string Manager { get; set; }
    }

