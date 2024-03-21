using Newtonsoft.Json;

namespace CampaignManagementTool.Shared;

/// <summary>
/// Represents a campaign entity.
/// </summary>
public class Campaign
{
    /// <summary>
    /// Gets or sets the campaign code.
    /// </summary>
    [JsonProperty("campaignCode")]
    public string CampaignCode { get; set; }

    /// <summary>
    /// Gets or sets the affiliate code associated with the campaign.
    /// </summary>
    [JsonProperty("affiliateCode")]
    public string AffiliateCode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the campaign requires approval.
    /// </summary>
    [JsonProperty("requiresApproval")]
    public bool RequiresApproval { get; set; }

    /// <summary>
    /// Gets or sets the rules associated with the campaign.
    /// </summary>
    [JsonProperty("rules")]
    public string? Rules { get; set; }

    /// <summary>
    /// Gets or sets the URL pointing to the rules document for the campaign.
    /// </summary>
    [JsonProperty("rulesUrl")]
    public string? RulesUrl { get; set; }

    /// <summary>
    /// Gets or sets the producer code associated with the campaign.
    /// </summary>
    [JsonProperty("producerCode")]
    public string? ProducerCode { get; set; }

    /// <summary>
    /// Gets or sets the expiry date of the campaign.
    /// </summary>
    [JsonProperty("expiryDays")]
    public string ExpiryDays { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the campaign is deleted.
    /// </summary>
    [JsonProperty("isDeleted")]
    public bool isDeleted { get; set; }
}