using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using DepartmentGrpc.Proto;
using System.ComponentModel.DataAnnotations;

public class DepartmentGrpcServices : DepartmentService.DepartmentServiceBase
{
    private readonly IMediator _mediator;

    public DepartmentGrpcServices (IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<CreateResponse> CreateDepartment(CreateRequest request, ServerCallContext context)
    {
        try
        {

            var createCommand = new CreateCommand
            {
                Name = request.Name,
                Manager = request.Manager,
            };
            var Dept = await _mediator.Send(new CreateDepartmentCommand(createCommand));
            
            return new CreateResponse { Success = Dept };
           
        }
        catch (ValidationException ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }

        catch (Exception ex)
        {
            throw new RpcException (new Status(StatusCode.InvalidArgument, ex.Message));    
        }

    }

    public override async Task<DepartmentResponse> GetDepartmentById(ByIdRequest request, ServerCallContext context)
    {

        try
        {

            var Dept = await _mediator.Send(new GetDepartmentByIdQuery(request.Id));

            return new DepartmentResponse
            {
                Id = Dept.Id,
                Name = Dept.Name,
                Manager = Dept.Manager,
                CreatedDate = Dept.CreatedDate.ToString("yyyy-MM-dd"),
                LastUpdate = Timestamp.FromDateTime((DateTime)Dept.LastUpdate),
            };
        }
        catch (ValidationException ex)
        {
            throw new RpcException (new Status (StatusCode.InvalidArgument, ex.Message));
        }
        catch(Exception ex)
        {
            throw new RpcException (new Status(StatusCode.InvalidArgument,ex.Message));
        }

    }

    public override async Task<UpdateResponse> UpdateDepartment(UpdateRequest request, ServerCallContext context)
    {
        try
        {
            var updateCommand = new UpdateCommand
            {
                Id = request.Id,
                Name = request.Name,
                Manager = request.Manager,
            };

            var success = await _mediator.Send (new UpdateDepartmentCommand(updateCommand));

            return new UpdateResponse
            {
                Success = success,
            };
        }
        catch (ValidationException ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch( Exception ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }

    }

    public override async Task<DepartmentsResponse> GetAllDepartments(Empty request, ServerCallContext context)
    {
        var Departments = await _mediator.Send(new GetAllDepartmentsQuery());

        var response = new DepartmentsResponse();

        response.Depts.AddRange(Departments.Select(d => new DepartmentResponse
        {
            Id = d.Id,
            Name = d.Name,
            Manager = d.Manager,
            CreatedDate = d.CreatedDate.ToString("yyyy-MM-dd"),
            LastUpdate = d.LastUpdate.HasValue? Timestamp.FromDateTime(DateTime.SpecifyKind(d.LastUpdate.Value, DateTimeKind.Utc)): null,
        }));

        return response;
    }

    public override async Task<DeleteResponse> DeleteDepartment(ByIdRequest request, ServerCallContext context)
    {
        var success = await _mediator.Send(new DeleteDepartmentCommand(request.Id));

        return new DeleteResponse { Success = success, };
    }
}