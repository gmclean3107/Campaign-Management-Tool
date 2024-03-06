using Bogus;
using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

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
        if (ValidateInput(campaign))
        {
            _campaigns.Add(campaign);
        }
        else 
        {
            throw new InvalidOperationException("Invalid campaign.");
        }
    }

    public async Task<Campaign?> GetById(string id)
    {
        return _campaigns.FirstOrDefault(c => c.CampaignCode == id);
    }

    public async Task Update(string id,Campaign campaign)
    {
        if (campaign == null) throw new ArgumentNullException(nameof(campaign));

        var existing = await GetById(id);

        if (existing != null && ValidateInput(campaign))
        {
            //Update existing by replacing the item in the list - not really an update but works for testing ;)
            _campaigns.Remove(existing);
            _campaigns.Add(campaign);
        }
        else
        {
            //Trying to update an incorrect code?
            throw new InvalidOperationException("Invalid campaign or ID doesn't exist.");
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
        List<Campaign> filteredCampaigns = new();
        if (filter == 0 && code == "") { filteredCampaigns = _campaigns; }

        foreach (Campaign campaign in _campaigns) 
        {
            if (code != "" && (campaign.CampaignCode.Contains(code) || campaign.AffiliateCode.Contains(code) || campaign.ProducerCode.Contains(code)))
            {
                if (filter != 0)
                {
                    switch (filter)
                    {
                        case 1:
                            if (campaign.RequiresApproval == true)
                            {
                                filteredCampaigns.Add(campaign);
                            }
                            break;
                        case 2:
                            if (campaign.RequiresApproval == false)
                            {
                                filteredCampaigns.Add(campaign);
                            }
                            break;
                        case 3:
                            if (campaign.isDeleted == false)
                            {
                                filteredCampaigns.Add(campaign);
                            }
                            break;
                        case 4:
                            if (campaign.isDeleted == true)
                            {
                                filteredCampaigns.Add(campaign);
                            }
                            break;
                        default: break;
                    }
                }
                else
                {
                    filteredCampaigns.Add(campaign);
                }
            }
            else if (code == ""){
                if (filter != 0)
                {
                    switch (filter)
                    {
                        case 1:
                            if (campaign.RequiresApproval == true)
                            {
                                filteredCampaigns.Add(campaign);
                            }
                            break;
                        case 2:
                            if (campaign.RequiresApproval == false)
                            {
                                filteredCampaigns.Add(campaign);
                            }
                            break;
                        case 3:
                            if (campaign.isDeleted == false)
                            {
                                filteredCampaigns.Add(campaign);
                            }
                            break;
                        case 4:
                            if (campaign.isDeleted == true)
                            {
                                filteredCampaigns.Add(campaign);
                            }
                            break;
                        default: break;
                    }
                }
            }
        }

        if (sort != 0)
        {
            switch (sort)
            {
                case 1:
                    filteredCampaigns = filteredCampaigns.OrderBy(campaign => campaign.CampaignCode).ToList();
                    break;
                case 2:
                    filteredCampaigns = filteredCampaigns.OrderByDescending(campaign => campaign.CampaignCode).ToList();
                    break;
                case 3:
                    filteredCampaigns = filteredCampaigns.OrderBy(campaign => DateTime.ParseExact(campaign.ExpiryDays, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
                    break;
                case 4:
                    filteredCampaigns = filteredCampaigns.OrderByDescending(campaign => DateTime.ParseExact(campaign.ExpiryDays, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
                    break;
                default: break;
            }
        }
        return Task.FromResult(filteredCampaigns);
    }

    public Task<List<Campaign>> ExportToCsv()
    {
        var response = _campaigns;

        try
        {
            if (response != null)
            {
                string exportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CsvExports/AllCampaigns.csv");
                using (var writer = new StreamWriter(exportPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(response);
                }
            }
            else
            {
                Console.WriteLine($"Campaign with id not found.");
                return null;
            }

        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            Console.WriteLine($"An error occurred while exporting campaign to CSV: {ex.Message}");
            return null;
        }
        return Task.FromResult(response);
    }

    public Task<List<Campaign>> ExportToCsvFiltered(string code, int filter, int sort)
    {
        var response = CampaignSearchFilter(code, filter, sort).Result;

        try
        {
            if (response != null)
            {
                using (var writer = new StreamWriter("CsvExports/FilteredCampaigns.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(response);
                }
            }
            else
            {
                Console.WriteLine($"Campaign with id not found.");
            }

        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            Console.WriteLine($"An error occurred while exporting campaign to CSV: {ex.Message}");
        }
        return Task.FromResult(response);
    }



    public Task<Campaign?> ExportToCsvSingle(string id)
    {
        var response = GetById(id);

        try
        {
            if (response != null)
            {
                using (var writer = new StreamWriter("CsvExports/SingleCampaign.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<Campaign>();
                    csv.NextRecord();

                    csv.WriteRecord(response.Result);
                }
                return response;
            }
            else
            {
                Console.WriteLine($"Campaign with id not found.");
                return null;
            }

        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            Console.WriteLine($"An error occurred while exporting campaign to CSV: {ex.Message}");
            return null;
        }
    }

    private bool ValidateInput(Campaign Model)
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(Model.CampaignCode))
        {
            isValid = false;
        }
        if (string.IsNullOrWhiteSpace(Model.AffiliateCode))
        {
            isValid = false;
        }
        if (string.IsNullOrWhiteSpace(Model.ExpiryDays))
        {
            isValid = false;
        }
        if ((!string.IsNullOrWhiteSpace(Model.Rules) && Model.Rules.Length > 1000) || (string.IsNullOrWhiteSpace(Model.Rules) && !string.IsNullOrWhiteSpace(Model.RulesUrl)))
        {
            isValid = false;
        }
        string urlPattern = @"^((https?|ftp):\/\/)?(www\.)?[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,}[^\s]*$";
        if ((!string.IsNullOrWhiteSpace(Model.Rules) && string.IsNullOrWhiteSpace(Model.RulesUrl)) || ((!string.IsNullOrWhiteSpace(Model.Rules)) && !string.IsNullOrWhiteSpace(Model.RulesUrl) && !Regex.IsMatch(Model.RulesUrl, urlPattern)))
        {
            isValid = false;
        }
        if (!string.IsNullOrWhiteSpace(Model.ProducerCode) && Model.ProducerCode.Length > 1000)
        {
            isValid = false;
        }

        return isValid;
    }
}