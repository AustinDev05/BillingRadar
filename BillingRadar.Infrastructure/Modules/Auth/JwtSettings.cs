using System;
using System.Collections.Generic;
using System.Text;

namespace BillingRadar.Infrastructure.Modules.Auth
{
    public class JwtSettings
    {
        public string Key { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int DurationInMinutes { get; init; }
    }
}
