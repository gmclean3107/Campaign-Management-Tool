using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Repositories.Interfaces;

public interface ICampaignRepository
{
    Task<List<Campaign>> GetAll();
    Task Add(Campaign campaign);
    Task<Campaign?> GetById(string id);
    Task Update(string id,Campaign campaign);
}