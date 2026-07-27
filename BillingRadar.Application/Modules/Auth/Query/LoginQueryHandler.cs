using System;
using System.Threading;
using System.Threading.Tasks;
using BillingRadar.Domain.Repositories;
using MediatR;
using BillingRadar.Application.Shared;

namespace BillingRadar.Application.Modules.Auth.Query
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<LoginQueryResponse>>
    {
        private readonly JwtSettings _jwtSettings;

        public LoginQueryHandler(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Result<LoginQueryResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !user.VerificarPassword(request.Password))
            {
                return Result<LoginQueryResponse>.Failure("Invalid email or password.");
            }

            var token = _jwtProvider.Generate(user);
            return Result<LoginQueryResponse>.Success(new LoginQueryResponse { Token = token });
        }
    }
}
