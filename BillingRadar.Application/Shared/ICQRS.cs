using MediatR;

namespace BillingRadar.Application.Shared
{
    // Commands: Retornan Result con ID o Valor de afectación o simplemente Result de éxito/fallo
    public interface ICommand : IRequest<Result> { }
    public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

    // Queries: Retornan Result con DTO de lectura
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
}
