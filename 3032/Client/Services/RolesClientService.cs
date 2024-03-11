using System.Net.Http.Json;
using CampaignManagementTool.Server;

namespace CampaignManagementTool.Client.Services;

public class RolesClientService
{
    private List<Role> _currentRoles = new List<Role>();
    
    private readonly HttpClient _httpClient;

    public RolesClientService(HttpClient  httpClient)
    {
        _httpClient = httpClient;
    }
    
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

    public async Task<bool> HasRole(string role)
    {
        var roles = await GetRoles();

        return roles.Any(r => r.Name == role);
    }
}