using BillingRadar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingRadar.Application.Interfaces
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
