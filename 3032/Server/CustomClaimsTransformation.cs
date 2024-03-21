using System.Security.Claims;
using CampaignManagementTool.Server;
using CampaignManagementTool.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.JsonWebTokens;


/// <summary>
/// Implements a custom claims transformation to add roles to the claims principal based on user roles obtained from the authorization service.
/// </summary>
internal sealed class CustomClaimsTransformation : IClaimsTransformation
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomClaimsTransformation"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    public CustomClaimsTransformation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Transforms the claims principal by adding roles based on the user's roles obtained from the authorization service.
    /// </summary>
    /// <param name="principal">The original claims principal.</param>
    /// <returns>The transformed claims principal.</returns>
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not { IsAuthenticated: true } ||
            principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
            principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
        {
            return principal;
        }

        using IServiceScope scope = _serviceProvider.CreateScope();

        var authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

        var identityId = principal.GetIdentityId();
        var name = principal.GetDisplayName();
        var userRoles = await authorizationService.GetRolesForUserAsync(identityId,name);

        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, identityId));

        foreach (var role in userRoles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        }

        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}