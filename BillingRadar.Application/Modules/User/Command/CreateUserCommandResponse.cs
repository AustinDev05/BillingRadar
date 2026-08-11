namespace BillingRadar.Application.Modules.User.Command
{
    public record CreateUserCommandResponse(string Name, string Surname, string Email, bool Status);
}