using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CampaignManagementTool.Server.Services;

/// <summary>
/// Extension methods for ClaimsPrincipal.
/// </summary>
internal static class ClaimsPrincipalExtensions
{

    /// <summary>
    /// Gets the identity ID from the ClaimsPrincipal.
    /// </summary>
    /// <param name="principal">The ClaimsPrincipal.</param>
    /// <returns>The identity ID if available, otherwise throws an exception.</returns>
    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new ApplicationException("User identity is unavailable");
    }
}