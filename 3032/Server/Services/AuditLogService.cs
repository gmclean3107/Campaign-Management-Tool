using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Services;

/// <summary>
/// Service for retrieving audit logs related to campaigns.
/// </summary>
public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _repo;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditLogService"/> class.
    /// </summary>
    /// <param name="repo">The audit log repository.</param>
    public AuditLogService(IAuditLogRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves audit logs for a specific campaign.
    /// </summary>
    /// <param name="code">The campaign code.</param>
    /// <returns>The list of audit logs for the campaign.</returns>
    public async Task<List<AuditLog>> GetForCampaign(string code)
    {
        var results = await _repo.GetAllForCampaign(code);
        return results;
    }
}