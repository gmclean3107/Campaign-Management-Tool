namespace CampaignManagementTool.Server;

public class User
{
    public string Name { get; set; }
    public string IdentityId { get; set; }
    public List<Role> Roles { get; set; }
}