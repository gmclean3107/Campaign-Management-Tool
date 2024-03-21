using CampaignManagementTool.Server;
using CampaignManagementTool.Server.Repositories.Interfaces;

/// <summary>
/// Service responsible for handling user authorization.
/// </summary>
public sealed class AuthorizationService
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheService _cacheService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationService"/> class.
    /// </summary>
    /// <param name="userRepository">The repository for user-related operations.</param>
    /// <param name="cacheService">The cache service for caching user roles.</param>
    public AuthorizationService(IUserRepository userRepository, ICacheService cacheService)
    {
        _userRepository = userRepository;
        _cacheService = cacheService;
    }

    /// <summary>
    /// Retrieves the roles assigned to a user identified by their identity ID.
    /// </summary>
    /// <param name="identityId">The identity ID of the user.</param>
    /// <param name="name">The name of the user.</param>
    /// <returns>A list of roles assigned to the user.</returns>
    public async Task<List<Role>> GetRolesForUserAsync(string identityId, string? name = null)
    {
        string cacheKey = $"auth:roles-{identityId}";
        var cachedRoles = _cacheService.Get<List<Role>>(cacheKey);

        if (cachedRoles is not null)
        {
            return cachedRoles;
        }

        //Cache Miss - get directly from the db
        var user = await _userRepository.GetById(identityId);

        if (user == null)
        {
            //No User Found - create a new user with read permissions
            var newUser = new User()
            {
                Name = name ?? "No Name",
                IdentityId = identityId,
                Roles = new List<Role>()
                {
                    Role.Viewer
                }
            };

            await _userRepository.Add(newUser);

            user = newUser;
        }
        
        var roles = user.Roles;

        //Update the cache so we don't have to do this check again
        _cacheService.Set(cacheKey, roles,TimeSpan.FromMinutes(5));

        return roles;
    }
}