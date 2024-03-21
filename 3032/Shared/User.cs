namespace CampaignManagementTool.Server;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the identity ID of the user.
    /// </summary>
    public string IdentityId { get; set; }

    /// <summary>
    /// Gets or sets the roles assigned to the user.
    /// </summary>
    public List<Role> Roles { get; set; }
}