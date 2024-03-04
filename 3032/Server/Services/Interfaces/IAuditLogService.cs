using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Services;

public interface IAuditLogService
{
    Task<List<AuditLog>> GetForCampaign(string code);
}