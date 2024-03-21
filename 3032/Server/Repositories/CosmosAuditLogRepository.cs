using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;
using Microsoft.Azure.Cosmos;

namespace CampaignManagementTool.Server.Repositories;

/// <summary>
/// Repository for managing audit logs stored in Cosmos DB.
/// </summary>
public class CosmosAuditLogRepository : CosmosDbRepository<AuditLog>, IAuditLogRepository
{
    private const string DatabaseId = "CampaignManagementTool";
    private const string ContainerId = "AuditLogs";

    /// <summary>
    /// Constructs a new instance of <see cref="CosmosAuditLogRepository"/>.
    /// </summary>
    /// <param name="configuration">The configuration instance.</param>
    public CosmosAuditLogRepository(IConfiguration configuration) : base(configuration, DatabaseId, ContainerId)
    {
    }

    /// <summary>
    /// Retrieves all audit logs associated with a specific campaign.
    /// </summary>
    /// <param name="campaign">The campaign code.</param>
    /// <returns>The list of audit logs for the specified campaign.</returns>
    public async Task<List<AuditLog>> GetAllForCampaign(string campaign)
    {
        var sql = $"SELECT * FROM c where c.payload.CampaignCode = '{campaign}' ORDER BY c.payload.AddedDate DESC";
        var query = new QueryDefinition(sql);

        return await GetFromQueryDefinition(query);

    }

    /// <summary>
    /// Converts an <see cref="AuditLog"/> instance to a <see cref="CosmosRecord{T}"/> instance.
    /// </summary>
    /// <param name="payload">The audit log payload.</param>
    /// <returns>The Cosmos record containing the audit log payload.</returns>
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