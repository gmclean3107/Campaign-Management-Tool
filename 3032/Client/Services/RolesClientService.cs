using System.Net.Http.Json;
using CampaignManagementTool.Server;

namespace CampaignManagementTool.Client.Services;

/// <summary>
/// Service for interacting with user roles.
/// </summary>
public class RolesClientService
{
    private List<Role> _currentRoles = new List<Role>();
    
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor for the Roles Client Service
    /// </summary>
    /// <param name="httpClient">HttpClient instance</param>
    public RolesClientService(HttpClient  httpClient)
    {
        _httpClient = httpClient;
    }
    
    /// <summary>
    /// Get roles that the current user has.
    /// </summary>
    /// <returns>Array of roles containing the roles the user possesses; otherwise, an empty array of Roles.</returns>
    public async Task<List<Role>> GetRoles()
    {
        if (_currentRoles.Count > 0)
        {
            return _currentRoles;
        }
        
        var campaigns = await _httpClient.GetFromJsonAsync<Role[]>($"Me/roles");

        _currentRoles = (campaigns ?? Array.Empty<Role>()).ToList();

        return _currentRoles;
    }

    /// <summary>
    /// Checks if user has the specified role.
    /// </summary>
    /// <param name="role">Required role.</param>
    /// <returns>True if user has the role; otherwise, returns false.</returns>
    public async Task<bool> HasRole(string role)
    {
        var roles = await GetRoles();

        return roles.Any(r => r.Name == role);
    }
}