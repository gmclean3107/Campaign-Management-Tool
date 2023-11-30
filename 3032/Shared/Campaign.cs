using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CampaignManagementTool.Shared;

public class Campaign
{
    [JsonProperty("campaignCode")]
    public string CampaignCode { get; set; }
    [JsonProperty("affiliateCode")]
    public string AffiliateCode { get; set; }
    [JsonProperty("requiresApproval")]
    public bool RequiresApproval { get; set; }
    [JsonProperty("rules")]
    public string? Rules { get; set; }
    [JsonProperty("rulesUrl")]
    public string? RulesUrl { get; set; }
    [JsonProperty("producerCode")]
    public string? ProducerCode { get; set; }
}