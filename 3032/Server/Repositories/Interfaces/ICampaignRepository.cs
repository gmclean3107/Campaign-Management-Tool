using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Repositories.Interfaces;

public interface ICampaignRepository
{
    Task<List<Campaign>> GetAll();
    Task Add(Campaign campaign);
    Task<Campaign?> GetById(string id);
    Task<List<Campaign>> CampaignSearchFilter(string code, int filter, int sort);
    Task Update(string id,Campaign campaign);
    Task<byte[]> ExportToCsv();
    Task<byte[]> ExportToCsvFiltered(string code, int filter, int sort);
    Task<byte[]> ExportToCsvSingle(string id);
}