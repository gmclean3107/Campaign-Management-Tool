using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;
using Microsoft.Azure.Cosmos;

namespace CampaignManagementTool.Server.Repositories;

public class CosmosAuditLogRepository : CosmosDbRepository<AuditLog>, IAuditLogRepository
{
    private const string DatabaseId = "CampaignManagementTool";
    private const string ContainerId = "AuditLogs";

    
    public CosmosAuditLogRepository(IConfiguration configuration) : base(configuration, DatabaseId, ContainerId)
    {
    }

    public async Task<List<AuditLog>> GetAllForCampaign(string campaign)
    {
        var sql = $"SELECT * FROM c where c.payload.CampaignCode = '{campaign}' ORDER BY c.payload.AddedDate DESC";
        var query = new QueryDefinition(sql);

        return await GetFromQueryDefinition(query);

    }
    
    protected override CosmosRecord<AuditLog> ToCosmosRecord(AuditLog payload)
    {
        return new CosmosRecord<AuditLog>()
        {
            Id = payload.Id.ToString(),
            PartitionKey = payload.CampaignCode,
            Payload = payload
        };
    }
}