using Newtonsoft.Json;

namespace CampaignManagementTool.Shared;

/// <summary>
/// Represents search filters used for filtering and sorting campaigns.
/// </summary>
public class SearchFilters
{
    /// <summary>
    /// Gets or sets the search criteria.
    /// </summary>
    [JsonProperty("search")]
    public string Search {  get; set; }

    /// <summary>
    /// Gets or sets the filter code.
    /// </summary>
    [JsonProperty("filterCode")]
    public int Filter {  get; set; }

    /// <summary>
    /// Gets or sets the sort code.
    /// </summary>
    [JsonProperty("sortCode")]
    public int Sort { get; set; }
}
