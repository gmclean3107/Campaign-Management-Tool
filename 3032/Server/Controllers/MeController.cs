using CampaignManagementTool.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace CampaignManagementTool.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class MeController : Controller
{
    private readonly AuthorizationService _authorizationService;
    private readonly IUserContext _userContext;

    /// <summary>
    /// Constructor for MeController
    /// </summary>
    /// <param name="authorizationService"></param>
    /// <param name="userContext"></param>
    public MeController(AuthorizationService authorizationService, IUserContext userContext)
    {
        _authorizationService = authorizationService;
        _userContext = userContext;
    }

    /// <summary>
    /// Gets user ID, name and roles.
    /// </summary>
    /// <returns>Code 200 and user's roles.</returns>
    [HttpGet("roles")]
    public async Task<ActionResult> GetRoles()
    {
        var identityId = _userContext.IdentityId;
        var name = _userContext.Name;
        var userRoles = await _authorizationService.GetRolesForUserAsync(identityId,name);

        return Ok(userRoles);
    }

}