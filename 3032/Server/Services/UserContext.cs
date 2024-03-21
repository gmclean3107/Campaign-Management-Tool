using System.Security.Claims;

namespace CampaignManagementTool.Server.Services;

/// <summary>
/// Implementation of the IUserContext interface to provide user-related context information.
/// </summary>
internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Constructor for initializing the UserContext.
    /// </summary>
    /// <param name="httpContextAccessor">The IHttpContextAccessor instance.</param>
    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the name of the user from the HttpContext.
    /// </summary>
    public string Name =>
        _httpContextAccessor
            .HttpContext?
            .User.FindFirst("name")
            ?.Value ?? "Unknown";

    /// <summary>
    /// Gets the identity ID of the user from the HttpContext.
    /// </summary>
    public string IdentityId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetIdentityId() ??
        throw new ApplicationException("User context is unavailable");
}