using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Repositories.Interfaces;

public interface IAuditLogRepository
{
    Task<List<AuditLog>> GetAll();
    Task<List<AuditLog>> GetAllForCampaign(string campaign);
    Task Add(AuditLog log);

}