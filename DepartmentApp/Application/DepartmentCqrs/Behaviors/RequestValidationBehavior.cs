using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DepartmentsCqrs.Behaviours
{
    public class RequestValidationBehavior : Interceptor
    {
        public override async Task<TResponse>UnaryServerHandler <TRequest,TResponse>(
            TRequest request, ServerCallContext context , UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (ValidationException ex)
            {
                var errorMessage = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));

                throw new RpcException(new Status (StatusCode.InvalidArgument,errorMessage));
            }

            catch(Exception ex)
            {
                throw new RpcException (new Status (StatusCode.Internal, ex.Message));
            }
        } 

        
    }
}
