using BillingRadar.Application.Shared;

namespace BillingRadar.Application.Modules.Auth.Query
{
    public record LoginQuery(string Email, string Password) : IQuery<LoginQueryResponse>;
}
