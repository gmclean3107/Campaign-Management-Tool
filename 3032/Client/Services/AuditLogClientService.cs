using System.Net.Http.Json;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Client.Services;

/// <summary>
/// Service for interacting with audit logs.
/// </summary>
public class AuditLogClientService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor for AuditLogClientService.
    /// </summary>
    public AuditLogClientService(HttpClient  httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Retrieves audit logs for a campaign.
    /// </summary>
    /// <param name="code">The campaign code.</param>
    /// <returns>A list of audit logs.</returns>
    public async Task<List<AuditLog>> GetForCampaignAsync(string code)
    {
        var campaigns = await _httpClient.GetFromJsonAsync<AuditLog[]>($"Campaign/{code}/History");

        return (campaigns ?? Array.Empty<AuditLog>()).ToList();
    }
}