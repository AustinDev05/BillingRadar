using BillingRadar.Application.Interfaces;
using BillingRadar.Application.Shared;
using BillingRadar.Domain.Repositories;
using MediatR;

namespace BillingRadar.Application.Modules.Auth.Query
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<LoginQueryResponse>>
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<LoginQueryResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !user.VerificarPassword(request.Password))
            {
                return Result<LoginQueryResponse>.Failure("Invalid email or password.");
            }

            var token = _jwtProvider.Generate(user);
            return Result<LoginQueryResponse>.Success(new LoginQueryResponse(token, user.Id.ToString()));
        }
    }
}
