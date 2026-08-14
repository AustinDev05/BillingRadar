using BillingRadar.Application.Shared;
using FluentValidation;
using MediatR;

namespace BillingRadar.Application.Shared.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            if (failures.Count != 0)
            {
                var errorMessage = string.Join("; ", failures.Select(f => f.ErrorMessage));
                return CreateFailureResult(errorMessage);
            }

            return await next();
        }

        private static TResponse CreateFailureResult(string errorMessage)
        {
            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(errorMessage);
            }

            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse);
                var failureMethod = resultType.GetMethod(nameof(Result.Failure), new[] { typeof(string) });
                if (failureMethod != null)
                {
                    return (TResponse)failureMethod.Invoke(null, new object[] { errorMessage })!;
                }
            }

            throw new InvalidOperationException($"El tipo de respuesta {typeof(TResponse).Name} no puede ser instanciado como Result.");
        }
    }
}
