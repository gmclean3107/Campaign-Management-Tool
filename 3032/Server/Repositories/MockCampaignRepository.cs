using Bogus;
using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;
using Microsoft.Azure.Cosmos;

namespace CampaignManagementTool.Server.Repositories;

/// <summary>
/// Campaign Repository based on an in memory list. This does not communicate with any database and should only be used for testing
/// </summary>
public class MockCampaignRepository : ICampaignRepository
{
    private readonly List<Campaign> _campaigns;

    public MockCampaignRepository()
    {
        _campaigns = GetFakeData();
    }

    public async Task<List<Campaign>> GetAll()
    {
        return _campaigns;
    }


    public async Task Add(Campaign campaign)
    {
        if (campaign == null) throw new ArgumentNullException(nameof(campaign));

        _campaigns.Add(campaign);
    }

    public async Task<Campaign?> GetById(string id)
    {
        return _campaigns.FirstOrDefault(c => c.CampaignCode == id);
    }

    public async Task Update(string id,Campaign campaign)
    {
        if (campaign == null) throw new ArgumentNullException(nameof(campaign));

        var existing = await GetById(id);

        if (existing != null)
        {
            //Update existing by replacing the item in the list - not really an update but works for testing ;)
            _campaigns.Remove(existing);
            _campaigns.Add(campaign);
        }
        else
        {
            //Trying to update an incorrect code?
        }
    }

    /// <summary>
    /// Use the Bogus Library to generate some fake data
    /// </summary>
    /// <returns></returns>
    private static List<Campaign> GetFakeData()
    {
        var yesNo = new List<bool> { true, false };

        var fakeData = new Faker<Campaign>()
            .RuleFor(c => c.CampaignCode, f => f.Random.Replace("###?#**"))
            .RuleFor(c => c.AffiliateCode, f => f.Random.Replace("****-****-****"))
            .RuleFor(c => c.RequiresApproval, f => f.PickRandom(yesNo))
            .RuleFor(c => c.Rules, f => f.Lorem.Lines(1))
            .RuleFor(c => c.RulesUrl, f => f.Internet.Url())
            .RuleFor(c => c.ExpiryDays, f => f.Date.ToString())
            .RuleFor(c => c.isDeleted, f => f.PickRandom(yesNo));

        return fakeData.Generate(20);
    }

    public Task<List<Campaign>> CampaignSearchFilter(string code, int filter, int sort)
    {
        return null;
    }

    public Task<List<Campaign>> ExportToCsv()
    {
        return null;
    }

    public Task<List<Campaign>> ExportToCsvFiltered(string code, int filter, int sort)
    {
        return null;
    }
}