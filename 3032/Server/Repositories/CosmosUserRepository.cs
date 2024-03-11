using CampaignManagementTool.Server.Repositories.Interfaces;

namespace CampaignManagementTool.Server.Repositories;

public class CosmosUserRepository : CosmosDbRepository<User>, IUserRepository
{
    private const string DatabaseId = "CampaignManagementTool";
    private const string ContainerId = "Users";
    
    public CosmosUserRepository(IConfiguration configuration) : base(configuration, DatabaseId, ContainerId)
    {
    }
    
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