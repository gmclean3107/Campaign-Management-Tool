using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _repo;

    public AuditLogService(IAuditLogRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<AuditLog>> GetForCampaign(string code)
    {
        var results = await _repo.GetAllForCampaign(code);
        return results;
    }
}