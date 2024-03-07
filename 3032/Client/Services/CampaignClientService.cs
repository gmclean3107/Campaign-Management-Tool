using System.Net;
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
        var response = await _httpClient.GetAsync($"Campaign/{id}");

        if (response.IsSuccessStatusCode)
        {
            var campaign = await response.Content.ReadFromJsonAsync<Campaign>();
            return campaign;
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        else
        {
            throw new HttpRequestException($"Failed to get campaign. Status code: {response.StatusCode}");
        }
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


    public async Task<List<Campaign>> ExportToCsvAsync()
    {
        
        var result = await _httpClient.GetFromJsonAsync<Campaign[]>($"Campaign/Export");

        return (result ?? Array.Empty<Campaign>()).ToList();
    }

    public async Task<List<Campaign>> ExportToCsvFilteredAsync(SearchFilters searchFilter) 
    {
        var result = await _httpClient.GetFromJsonAsync<Campaign[]>($"Campaign/ExportFiltered?code={searchFilter.Search}&filter={searchFilter.Filter}&sort={searchFilter.Sort}");
        return (result ?? Array.Empty<Campaign>()).ToList();
    }

    public async Task ExportToCsvSingleAsync(string id) 
    {
        var result = await _httpClient.GetFromJsonAsync<Campaign>($"Campaign/ExportSingle?id={id}");
    }
}