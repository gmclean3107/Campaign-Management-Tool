using System.Security.Claims;

namespace CampaignManagementTool.Server.Services;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Name =>
        _httpContextAccessor
            .HttpContext?
            .User.FindFirst("name")
            ?.Value ?? "Unknown";

    public string IdentityId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetIdentityId() ??
        throw new ApplicationException("User context is unavailable");
}