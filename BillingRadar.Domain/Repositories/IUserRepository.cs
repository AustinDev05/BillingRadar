using BillingRadar.Domain.Entities;

namespace BillingRadar.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
    }
}
