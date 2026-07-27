using BillingRadar.Domain.Entities;

namespace BillingRadar.Application.Interfaces
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
