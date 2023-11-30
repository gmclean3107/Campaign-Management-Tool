using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Services.Interfaces;

public interface ICampaignService
{
    Task<List<Campaign>> GetAll();
    Task Add(Campaign campaign);
    Task<Campaign?> GetById(string id);
    Task Update(string id,Campaign campaign);
}