using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Server.Services.Interfaces;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Services;

public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _repo;

    public CampaignService(ICampaignRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<Campaign>> GetAll()
    {
        var results = await _repo.GetAll();
        return results;
    }

    public async Task Add(Campaign campaign)
    {
        await _repo.Add(campaign);
    }

    public async Task<Campaign?> GetById(string id)
    {
        return await _repo.GetById(id);
    }

    public async Task<List<Campaign>> CampaignSearch(string code) {
        var results = await _repo.CampaignSearch(code);
        return results;
    }

    public async Task<List<Campaign>> HandleFilter(int filter) {
        var results = await _repo.HandleFilter(filter);
        return results;
    }

    public async Task Update(string id, Campaign campaign)
    {
        await _repo.Update(id,campaign);
    }
}