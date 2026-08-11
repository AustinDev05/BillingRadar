using BillingRadar.Application.Shared;
using BillingRadar.Domain.Repositories;
using MediatR;

namespace BillingRadar.Application.Modules.User.Query
{
    public class UserQueryHandler : IRequestHandler<UserQuery, Result<UserQueryResponse>>
    {
        private readonly IUserRepository _userRepository;

        public UserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserQueryResponse>> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0 || request == null)
            {
                return Result<UserQueryResponse>.Failure("Id de usuario no puede estar vacío.");
            }

            var userQuery = await _userRepository.GetByIdAsync(request.Id);

            if (userQuery == null)
            {
                return Result<UserQueryResponse>.Failure("Usuario no encontrado.");
            }

            return Result<UserQueryResponse>.Success(new UserQueryResponse(userQuery.Id, userQuery.Name, userQuery.Surname, userQuery.Email));
        }
    }
}