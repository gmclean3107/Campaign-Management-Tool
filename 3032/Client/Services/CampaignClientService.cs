using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CampaignManagementTool.Shared;
using static System.Net.WebRequestMethods;

namespace CampaignManagementTool.Client.Services;

public class CampaignClientService
{
    private readonly HttpClient _httpClient;

    public CampaignClientService(HttpClient  httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Campaign>> GetAllAsync()
    {
        var campaigns = await _httpClient.GetFromJsonAsync<Campaign[]>("Campaign");
        return (campaigns ?? Array.Empty<Campaign>()).ToList();
    }

    public async Task<Campaign?> GetByIdAsync(string id)
    {
        var campaign = await _httpClient.GetFromJsonAsync<Campaign>($"Campaign/{id}");
        return campaign;
    }

    public async Task<List<Campaign>> CampaignSearchFilterAsync(SearchFilters searchFilter)
    {
        var campaigns = await _httpClient.GetFromJsonAsync<Campaign[]>($"Campaign/Search?code={searchFilter.Search}&filter={searchFilter.Filter}&sort={searchFilter.Sort}");
        return (campaigns ?? Array.Empty<Campaign>()).ToList();
    }

    public async Task<bool> AddAsync(Campaign model)
    {
        // var json = JsonSerializer.Serialize(model);
        // var content = new StringContent(json, Encoding.UTF8, "application/json");
        var result = await _httpClient.PostAsJsonAsync("Campaign", model);

        return result.IsSuccessStatusCode;
    }


    public async Task<bool> UpdateAsync(Campaign model)
    {
        // var json = JsonSerializer.Serialize(model);
        // var content = new StringContent(json, Encoding.UTF8, "application/json");
        var result = await _httpClient.PutAsJsonAsync("Campaign", model);

        return result.IsSuccessStatusCode;
    }


    public async Task<List<Campaign>> ExportToCsvAsync(bool isSingleCampaign)
    {
        
        var result = await _httpClient.GetFromJsonAsync<Campaign[]>($"Campaign/Export?isSinglular={isSingleCampaign}");

        return (result ?? Array.Empty<Campaign>()).ToList();
    }
}