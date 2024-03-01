using Bogus;
using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

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

 
    public static List<Campaign> GetFakeData()
    {
        // Path to the JSON file containing test data
        string testDataFilePath = "Config/TestData.json";

        // Read JSON file content
        string jsonData = File.ReadAllText(testDataFilePath);

        // Deserialize JSON to List<Campaign>
        List<Campaign> campaigns = JsonConvert.DeserializeObject<List<Campaign>>(jsonData);


        return campaigns;
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



    public Task<Campaign?> ExportToCsvSingle(string id)
    {
        return null;
    }
}