using System;
using System.Security.Claims;
using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
