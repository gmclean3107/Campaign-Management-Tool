using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CampaignManagementTool.Server.Services;

internal static class ClaimsPrincipalExtensions
{

    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new ApplicationException("User identity is unavailable");
    }
}