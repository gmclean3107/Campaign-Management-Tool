namespace CampaignManagementTool.Shared;

/// <summary>
/// Represents an audit log entry for tracking changes made to a campaign.
/// </summary>
public class AuditLog
{
    /// <summary>
    /// Gets or sets the unique identifier of the audit log entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets information about the user who added the audit log entry.
    /// </summary>
    public UserInfo AddedBy { get; set; }

    /// <summary>
    /// Gets or sets the campaign code associated with the audit log entry.
    /// </summary>
    public string CampaignCode { get; set; }

    /// <summary>
    /// Gets or sets the list of update values indicating the changes made to the campaign.
    /// </summary>
    public List<UpdateValues> Updates { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the audit log entry was added.
    /// </summary>
    public DateTime AddedDate { get; set; }

    /// <summary>
    /// Represents the values before and after a field update in an audit log entry.
    /// </summary>
    public class UpdateValues
    {
        /// <summary>
        /// Gets or sets the name of the field that was updated.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the value of the field before the update.
        /// </summary>
        public string? ValueBefore { get; set; }

        /// <summary>
        /// Gets or sets the value of the field after the update.
        /// </summary>
        public string? ValueAfter { get; set; }
    }
}