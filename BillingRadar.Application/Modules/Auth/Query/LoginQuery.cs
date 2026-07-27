using BillingRadar.Application.Shared;
using MediatR;

namespace BillingRadar.Application.Modules.Auth.Query
{
    public record LoginQuery : IRequest<Result<LoginQueryResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
