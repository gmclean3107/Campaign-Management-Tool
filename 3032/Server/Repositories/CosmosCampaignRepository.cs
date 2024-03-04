using Microsoft.Azure.Cosmos;
using System.Configuration;
using System.Globalization;
using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;
using System.Net;
using CsvHelper;

namespace CampaignManagementTool.Server.Repositories
{

    public class CosmosCampaignRepository : CosmosDbRepository<Campaign>, ICampaignRepository
    {
        private const string DatabaseId = "CampaignManagementTool";
        private const string ContainerId = "Campaigns";


        public CosmosCampaignRepository(IConfiguration configuration) : base(configuration, DatabaseId, ContainerId)
        {
        }
        
        public async Task<List<Campaign>> CampaignSearchFilter(string code, int filter, int sort)
        {
            string filterString = "";
            string sortString = "";
            string queryString = "";
            QueryDefinition query;

            filterString = filter switch
            {
                1 => "c.payload.requiresApproval = true",
                2 => "c.payload.requiresApproval = false",
                3 => "c.payload.isDeleted = false",
                4 => "c.payload.isDeleted = true",
                _ => "1=1",
            };

            sortString = sort switch
            {
                1 => "ORDER BY c.id ASC",
                2 => "ORDER BY c.id DESC",
                3 => "ORDER BY c.payload.expiryDays ASC",
                4 => "ORDER BY c.payload.expiryDays DESC",
                _ => "AND 1=1",
            };

            if (code != "")
            {
                queryString = "SELECT * FROM c WHERE (CONTAINS(c.payload.campaignCode, @code) OR CONTAINS(c.payload.affiliateCode, @code) OR CONTAINS(c.payload.producerCode, @code)) AND " + filterString + " " + sortString;
                query = new QueryDefinition(queryString)
                    .WithParameter("@code", code);
            }
            else
            {
                query = new QueryDefinition("SELECT * FROM c WHERE " + filterString + " " + sortString);
            }

            return await GetFromQueryDefinition(query);
        }
        
        public async Task<List<Campaign>> ExportToCsvFiltered(string code, int filter, int sort)
        {
            var response = CampaignSearchFilter(code, filter, sort).Result;

            try
            {
                if (response != null)
                {
                    using (var writer = new StreamWriter("CsvExports/FilteredCampaigns.csv"))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(response);
                    }
                }
                else
                {
                    Console.WriteLine($"Campaign with id not found.");
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                Console.WriteLine($"An error occurred while exporting campaign to CSV: {ex.Message}");
            }
            return response;
        }
        
        public Task<Campaign?> ExportToCsvSingle(string id) 
        {
            var response = GetById(id);

            try
            {
                if (response != null)
                {
                    using (var writer = new StreamWriter("CsvExports/SingleCampaign.csv"))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteHeader<Campaign>();
                        csv.NextRecord();

                        csv.WriteRecord(response.Result);
                    }
                    return response;
                }
                else
                {
                    Console.WriteLine($"Campaign with id not found.");
                    return null;
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                Console.WriteLine($"An error occurred while exporting campaign to CSV: {ex.Message}");
                return null;
            }
        }



        protected override CosmosRecord<Campaign> ToCosmosRecord(Campaign payload)
        {
            return new CosmosRecord<Campaign>()
            {
                Id = payload.CampaignCode,
                PartitionKey = "*",
                Payload = payload
            };
        }
    }
}
