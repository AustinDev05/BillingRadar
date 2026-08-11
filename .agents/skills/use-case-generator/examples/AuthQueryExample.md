# Real Project Reference: Auth Query Use Case

Files in `BillingRadar.Application/Modules/Auth/Query/`:

### 1. Request (`LoginQuery.cs`)
```csharp
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
```

### 2. Response DTO (`LoginQueryResponse.cs`)
> **Note**: Standard project convention requires immutable `record` types with camelCase constructor parameters.

```csharp
namespace BillingRadar.Application.Modules.Auth.Query
{
    public record LoginQueryResponse(string accessToken, string refreshToken);
}
```

### 3. Handler (`LoginQueryHandler.cs`)
```csharp
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
```
