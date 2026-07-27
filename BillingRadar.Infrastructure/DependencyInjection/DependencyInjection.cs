using BillingRadar.Infrastructure.Modules.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace BillingRadar.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<BillingRadar.Infrastructure.Persistence.ApplicationDbContext>(options =>
                options.UseNpgsql(dataSource));

            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            services.AddScoped<BillingRadar.Domain.Repositories.IUserRepository, BillingRadar.Infrastructure.Repositories.UserRepository>();
            services.AddScoped<BillingRadar.Application.Interfaces.IJwtProvider, BillingRadar.Infrastructure.Repositories.JwtProvider>();

            return services;
        }
    }
}
