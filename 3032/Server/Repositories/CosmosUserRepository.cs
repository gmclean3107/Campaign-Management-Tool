using CampaignManagementTool.Server.Repositories.Interfaces;

namespace CampaignManagementTool.Server.Repositories;

public class CosmosUserRepository : CosmosDbRepository<User>, IUserRepository
{
    private const string DatabaseId = "CampaignManagementTool";
    private const string ContainerId = "Users";
    
    /// <summary>
    /// Cosntructor for CosmosUserRepository
    /// </summary>
    /// <param name="configuration"></param>
    public CosmosUserRepository(IConfiguration configuration) : base(configuration, DatabaseId, ContainerId)
    {
    }

    /// <summary>
    /// Converts an <see cref="User"/> instance to a <see cref="CosmosRecord{User}"/> instance.
    /// </summary>
    /// <param name="payload">The user payload.</param>
    /// <returns>The Cosmos record containing the user payload.</returns>
    protected override CosmosRecord<User> ToCosmosRecord(User payload)
    {
        return new CosmosRecord<User>()
        {
            Id = payload.IdentityId.ToString(),
            PartitionKey = "*",
            Payload = payload
        };
    }
}