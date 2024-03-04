using System.Net.Http.Json;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Client.Services;

public class AuditLogClientService
{
    private readonly HttpClient _httpClient;

    public AuditLogClientService(HttpClient  httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<List<AuditLog>> GetForCampaignAsync(string code)
    {
        var campaigns = await _httpClient.GetFromJsonAsync<AuditLog[]>($"Campaign/{code}/History");

        return (campaigns ?? Array.Empty<AuditLog>()).ToList();
    }
}