using BillingRadar.Application.Shared;
using MediatR;

namespace BillingRadar.Application.Modules.User.Command
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
    {
        public Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result<CreateUserCommandResponse>.Success(new CreateUserCommandResponse(
                request.Name,
                request.Surname,
                request.Email,
                request.Status
                )));
        }
    }
}