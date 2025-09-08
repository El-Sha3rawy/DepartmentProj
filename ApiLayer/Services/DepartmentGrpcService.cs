using Shared.Proto;
using ApplicationLayer.DepartmentsCqrs.Commands;
using ApplicationLayer.DepartmentsCqrs.Queries;
using AutoMapper;
using FluentResults;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InfrastructureLayer;
using MediatR;
using Shared;
using FluentValidation;
namespace ApiLayer.Services
{
    public class DepartmentGrpcService : DepartmentService.DepartmentServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IRepository _Irepo; 
        
        

        public DepartmentGrpcService (
            IMediator mediator,
            IMapper mapper,
            IRepository Irepo
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _Irepo = Irepo;
            
        }

        public override async Task<CreateDeptResponse> CreateDept(CreateDeptRequest request,ServerCallContext context)
        {

            try
            {
                var check = await _Irepo.GetByName(request.Name);

                if (check)
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "This Name Is Already Existed"));

                var deptDto = _mapper.Map<DepartmentDto>(request);

                var command = await _mediator.Send(new CreateDeptCommand(deptDto));

                return new CreateDeptResponse
                {
                    Id = command.Id,
                    Name = command.Name,
                    Manager = command.Manager,
                    CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    LastUpdated = Timestamp.FromDateTime(DateTime.UtcNow.ToUniversalTime())

                };
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Unexpected error: {ex.Message}"));
            }
        }

        public override async Task<UpdateDeptResponse> UpdateDept(UpdateDeptRequest request, ServerCallContext context)
        {
            try
            {
                var Check = await _Irepo.GetByName(request.Name);

                if (Check == true)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "This Name Is Already Existed"));
                }

                var DeptDto = new DepartmentDto();
                _mapper.Map(request, DeptDto);
                var command = await _mediator.Send(new UpdateDeptCommand(request.Id, DeptDto));

                if (command == true)
                {

                    return new UpdateDeptResponse
                    {
                        RMessage = "The Department Is Updated Successfully "
                    };

                }
                return new UpdateDeptResponse
                {
                    RMessage = "There is a problem with Updating "
                };
            } 
            
            catch (RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Unexpected error{ex.Message}"));
            }
           

        }

        public override async Task<DeptResponse> GetDeptById(ByIdRequest request, ServerCallContext context)
        {


            var command = await _mediator.Send(new GetDeptByIdQuery(request.Id));
            if (command == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The Department Is not Found"));
            }

            var response = new DeptResponse();
            _mapper.Map(command,response);

            return response;
               
           
        }

        public override async Task<DeleteDeptResponse> DeleteDept(ByIdRequest request, ServerCallContext context)
        {

            try
            {

            
                var Check = await _mediator.Send(new DeleteDeptCommand(request.Id));

           

                 if (Check == true)
                 { 
                     return new DeleteDeptResponse
                     {
                         RMessage = " The Department Is Deleted Successfully "
                     };
                 }

                throw new RpcException(new Status(StatusCode.InvalidArgument, "There is A problem with deleting "));


            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Unexpected error: {ex.Message}"));
            }
        }
        

        public override async  Task<DeptsResponse> GetAllDepts(Shared.Proto.Empty request, ServerCallContext context)
        {
            var command = await _mediator.Send(new GetAllDeptQuery());

            var response = new DeptsResponse();

            response.Depts.AddRange(command.Select(d => new DeptResponse
            {
                Id = d.Id,
                Name = d.Name,
                Manager = d.Manager,
                CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                LastUpdated = Timestamp.FromDateTime(DateTime.UtcNow.ToUniversalTime())
            }));

            return response;

        }

    }
}
