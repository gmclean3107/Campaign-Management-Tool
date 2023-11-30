using Microsoft.Azure.Cosmos;
using System.Configuration;
using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;
using System.Net;

namespace CampaignManagementTool.Server.Repositories
{

    public class CosmosCampaignRepository : CosmosDbRepository<Campaign>, ICampaignRepository
    {
        public const string DatabaseId = "CampaignManagementTool";
        public const string ContainerId = "Campaigns";


        public CosmosCampaignRepository(IConfiguration configuration) : base(configuration, DatabaseId, ContainerId)
        {
        }

        protected override CosmosRecord<Campaign> ToCosmosRecord(Campaign payload)
        {
            return new CosmosRecord<Campaign>()
            {
                Id = payload.CampaignCode,
                paritionKey = "*",
                Payload = payload
            };
        }
    }
}
