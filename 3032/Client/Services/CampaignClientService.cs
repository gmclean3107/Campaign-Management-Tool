using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CampaignManagementTool.Shared;
using static System.Net.WebRequestMethods;

namespace CampaignManagementTool.Client.Services;

/// <summary>
/// Service for interacting with backend.
/// </summary>
public class CampaignClientService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor for CampaignClientService.
    /// </summary>
    /// <param name="httpClient">The HttpClient instance.</param>
    public CampaignClientService(HttpClient  httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Retrieves all campaigns.
    /// </summary>
    /// <returns>A list of all campaigns.</returns>
    public async Task<List<Campaign>> GetAllAsync()
    {
        var campaigns = await _httpClient.GetFromJsonAsync<Campaign[]>("Campaign");
        return (campaigns ?? Array.Empty<Campaign>()).ToList();
    }

    /// <summary>
    /// Retrieves a campaign by its ID.
    /// </summary>
    /// <param name="id">The ID of the campaign to retrieve.</param>
    /// <returns>The campaign with the specified ID, or null if not found.</returns>
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

    /// <summary>
    /// Performs the search/filter/sort function.
    /// </summary>
    /// <param name="searchFilter">JSON Object containing the values to be searched for</param>
    /// <returns>Filtered array of campaigns; otherwise, an empty list of campaigns</returns>
    public async Task<List<Campaign>> CampaignSearchFilterAsync(SearchFilters searchFilter)
    {
        var campaigns = await _httpClient.GetFromJsonAsync<Campaign[]>($"Campaign/Search?code={searchFilter.Search}&filter={searchFilter.Filter}&sort={searchFilter.Sort}");
        return (campaigns ?? Array.Empty<Campaign>()).ToList();
    }

    /// <summary>
    /// Adds a campaign to the database.
    /// </summary>
    /// <param name="model">Campaign to be added</param>
    /// <returns>Success Code 200</returns>
    public async Task<bool> AddAsync(Campaign model)
    {
        // var json = JsonSerializer.Serialize(model);
        // var content = new StringContent(json, Encoding.UTF8, "application/json");
        var result = await _httpClient.PostAsJsonAsync("Campaign", model);

        return result.IsSuccessStatusCode;
    }

    /// <summary>
    /// Updates an existing campaign.
    /// </summary>
    /// <param name="model">Campaign with the values to be updated.</param>
    /// <returns>Success Code 200</returns>
    public async Task<bool> UpdateAsync(Campaign model)
    {
        // var json = JsonSerializer.Serialize(model);
        // var content = new StringContent(json, Encoding.UTF8, "application/json");
        var result = await _httpClient.PutAsJsonAsync("Campaign", model);

        return result.IsSuccessStatusCode;
    }

    /// <summary>
    /// Exports all the campaigns to a csv file.
    /// </summary>
    /// <returns>Array of bytes that represents the campaigns in a csv format.</returns>
    /// <exception cref="Exception">Thrown if the export fails.</exception>
    public async Task<byte[]> ExportToCsvAsync()
    {
        
        var response = await _httpClient.GetAsync($"Campaign/Export");

        if (response.IsSuccessStatusCode)
        {
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        else
        {
            throw new Exception($"Failed to export CSV: {response.StatusCode}");
        }
    }
    /// <summary>
    /// Export filtered campaigns to a csv file.
    /// </summary>
    /// <param name="searchFilter">JSON Object containing the values to be used for filtering.</param>
    /// <returns>Array of bytes that represents the campaigns in a csv format.</returns>
    /// <exception cref="Exception">Thrown if the export fails.</exception>
    public async Task<byte[]> ExportToCsvFilteredAsync(SearchFilters searchFilter) 
    {
        var response = await _httpClient.GetAsync($"Campaign/ExportFiltered?code={searchFilter.Search}&filter={searchFilter.Filter}&sort={searchFilter.Sort}");
        if (response.IsSuccessStatusCode)
        {
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        else
        {
            throw new Exception($"Failed to export CSV: {response.StatusCode}");
        }
    }

    /// <summary>
    /// Export a single campaign to a csv file.
    /// </summary>
    /// <param name="id">ID of the Campaign to be exported</param>
    /// <returns>Array of bytes that represents the campaign in a csv format.</returns>
    /// <exception cref="Exception">Thrown if the export fails.</exception>
    public async Task<byte[]> ExportToCsvSingleAsync(string id) 
    {
        var response = await _httpClient.GetAsync($"Campaign/ExportSingle?id={id}");
        if (response.IsSuccessStatusCode)
        {
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        else
        {
            throw new Exception($"Failed to export CSV: {response.StatusCode}");
        }
    }
}