using BillingRadar.Application.Shared;

namespace BillingRadar.Application.Modules.Auth.Query
{
    public record LoginQuery : IQuery<LoginQueryResponse>
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
