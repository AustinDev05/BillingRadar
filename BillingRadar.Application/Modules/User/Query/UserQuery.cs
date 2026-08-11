using BillingRadar.Application.Shared;

namespace BillingRadar.Application.Modules.User.Query
{
    public record UserQuery(int Id)
        : IQuery<UserQueryResponse>;
}