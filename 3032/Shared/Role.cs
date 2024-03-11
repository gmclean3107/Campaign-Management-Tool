using System.Reflection.PortableExecutable;

namespace CampaignManagementTool.Server;

public class Role
{
    public static readonly  Role Viewer = new("Viewer");
    public static readonly  Role Editor = new("Editor");
    
    public string? Name { get; set; }

    //Default Constructor needed for serialization
    public Role()
    {
        
    }
    
    private Role(string name)
    {
        Name = name;
    }
}