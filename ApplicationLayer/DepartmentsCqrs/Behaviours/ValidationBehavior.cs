using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DepartmentsCqrs.Behaviours
{
    public class ValidationBehavior <TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse>  
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle (TRequest request,
            RequestHandlerDelegate<TResponse> next , CancellationToken cancellationToken)
        {
            if (_validators.Any ())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(V => V.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany (r=>r.Errors).Where (f=>f != null).ToList();

                if (failures.Count != 0 )
                {
                    throw new FluentValidation.ValidationException(failures);
                }
            }
            return await next();
        }
    
    }
}
