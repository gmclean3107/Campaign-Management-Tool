using Microsoft.Azure.Cosmos;
using System.Configuration;
using System.Globalization;
using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Shared;
using System.Net;
using CsvHelper;
using System.Text;

namespace CampaignManagementTool.Server.Repositories
{

    /// <summary>
    /// Repository for managing campaigns stored in Cosmos DB.
    /// </summary>
    public class CosmosCampaignRepository : CosmosDbRepository<Campaign>, ICampaignRepository
    {
        private const string DatabaseId = "CampaignManagementTool";
        private const string ContainerId = "Campaigns";

        /// <summary>
        /// Constructs a new instance of <see cref="CosmosCampaignRepository"/>.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        public CosmosCampaignRepository(IConfiguration configuration) : base(configuration, DatabaseId, ContainerId)
        {
        }

        /// <summary>
        /// Retrieves campaigns based on search filters.
        /// </summary>
        /// <param name="code">The search code.</param>
        /// <param name="filter">The filter criteria.</param>
        /// <param name="sort">The sorting criteria.</param>
        /// <returns>The list of campaigns matching the search and filter criteria.</returns>
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

        /// <summary>
        /// Exports filtered campaigns to a CSV byte array.
        /// </summary>
        /// <param name="code">The search code.</param>
        /// <param name="filter">The filter criteria.</param>
        /// <param name="sort">The sorting criteria.</param>
        /// <returns>The byte array containing the CSV data.</returns>
        public async Task<byte[]> ExportToCsvFiltered(string code, int filter, int sort)
        {
            var response = CampaignSearchFilter(code, filter, sort).Result;

            try
            {
                if (response != null)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(response);
                        writer.Flush();

                        return memoryStream.ToArray();
                    }
                }
                else
                {
                    Console.WriteLine($"Campaign with id not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while exporting campaign to CSV: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Exports a single campaign to a CSV byte array.
        /// </summary>
        /// <param name="id">The campaign ID.</param>
        /// <returns>The byte array containing the CSV data.</returns>
        public Task<byte[]> ExportToCsvSingle(string id) 
        {
            var response = GetById(id).Result;
            
            try
            {
                if (response != null)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteHeader<Campaign>();
                        csv.NextRecord();

                        csv.WriteRecord(response);
                        csv.NextRecord();

                        writer.Flush();

                        return Task.FromResult(memoryStream.ToArray());
                    }
                }
                else
                {
                    Console.WriteLine($"Campaign with id not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while exporting campaign to CSV: {ex.Message}");
            }

            return null;
        }


        /// <summary>
        /// Converts an <see cref="Campaign"/> instance to a <see cref="CosmosRecord{T}"/> instance.
        /// </summary>
        /// <param name="payload">The campaign payload.</param>
        /// <returns>The Cosmos record containing the campaign payload.</returns>
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
