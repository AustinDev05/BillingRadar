using BillingRadar.Application.Shared;

namespace BillingRadar.Application.Modules.User.Command
{
    public record CreateUserCommand(string Email, string Password, string Name, string Surname, string RepeatPassword, bool Status)
        : ICommand<CreateUserCommandResponse>;
}