using BillingRadar.Domain.Entities;

namespace BillingRadar.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
