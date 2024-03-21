using System.Reflection.PortableExecutable;

namespace CampaignManagementTool.Server;

public class Role
{
    public static readonly  Role Viewer = new("Viewer");
    public static readonly  Role Editor = new("Editor");

    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string? Name { get; set; }

    //Default Constructor needed for serialization
    public Role()
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    private Role(string name)
    {
        Name = name;
    }
}