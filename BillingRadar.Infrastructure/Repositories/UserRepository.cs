using BillingRadar.Domain.Entities;
using BillingRadar.Domain.Repositories;
using BillingRadar.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BillingRadar.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<User?> GetUserByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
