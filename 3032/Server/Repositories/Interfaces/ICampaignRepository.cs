using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Repositories.Interfaces;

public interface ICampaignRepository
{
    Task<List<Campaign>> GetAll();
    Task Add(Campaign campaign);
    Task<Campaign?> GetById(string id);
    Task<List<Campaign>> CampaignSearch(string code);
    Task Update(string id,Campaign campaign);
    Task<List<Campaign>> HandleFilter(int filter);
}