using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CampaignManagementTool.Shared;
public class SearchFilters
{
    [JsonProperty("search")]
    public string Search {  get; set; }

    [JsonProperty("filterCode")]
    public int Filter {  get; set; }

    [JsonProperty("sortCode")]
    public int Sort { get; set; }
}
