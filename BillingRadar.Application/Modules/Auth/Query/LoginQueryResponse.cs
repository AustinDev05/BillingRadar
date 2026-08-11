namespace BillingRadar.Application.Modules.Auth.Query
{
    public record LoginQueryResponse(string AccessToken, string RefreshToken);
}
