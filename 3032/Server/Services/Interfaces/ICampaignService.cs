using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Services.Interfaces;

public interface ICampaignService
{
    Task<List<Campaign>> GetAll();
    Task Add(Campaign campaign);
    Task<Campaign?> GetById(string id);
    Task<List<Campaign>> CampaignSearchFilter(string code, int filter, int sort);
    Task Update(string id,Campaign campaign);
    Task<List<Campaign>> ExportToCsv(bool isSingleCampaign);
}