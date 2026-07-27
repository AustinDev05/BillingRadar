using MediatR;
using BillingRadar.Application.Shared;

namespace BillingRadar.Application.Modules.Auth.Query
{
    public record LoginQuery : IRequest<Result<LoginQueryResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
