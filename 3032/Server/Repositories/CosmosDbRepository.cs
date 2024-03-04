using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Drawing.Text;
using System.Formats.Asn1;
using CsvHelper;
using Azure;
using System.Globalization;

namespace CampaignManagementTool.Server.Repositories
{
    public class CosmosRecord<T>
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty(PropertyName = "payload")]
        public T Payload { get; set; }
    }

    public abstract class CosmosDbRepository<T> where T : class
    {
        private readonly string _databaseId;
        private readonly string _containerId;
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        protected virtual CosmosRecord<T> ToCosmosRecord(T payload) => throw new NotImplementedException();

        public CosmosDbRepository(IConfiguration configuration, string databaseId, string containerId)
        {
            _databaseId = databaseId;
            _containerId = containerId;

            var endpointUri = configuration["CosmosDbSettings:EndpointUri"];
            var primaryKey = configuration["CosmosDbSettings:PrimaryKey"];

            if (string.IsNullOrEmpty(endpointUri) || string.IsNullOrEmpty(primaryKey))
            {
                throw new InvalidOperationException("Cosmos DB configuration is missing or invalid.");
            }

            _cosmosClient = new CosmosClient(endpointUri, primaryKey);
            _container = _cosmosClient.GetContainer(_databaseId, _containerId);
        }

        public async Task<T?> GetById(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<CosmosRecord<T>>(id, new PartitionKey("*"));
                return response.Resource.Payload;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        protected async Task<List<T>> GetFromQueryDefinition(QueryDefinition queryDefinition)
        {
            var iterator = _container.GetItemQueryIterator<CosmosRecord<T>>(queryDefinition);
            var results = new List<T>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.Resource.Select(r => r.Payload));
            }

            return results;
        }
        public async Task<List<T>> GetAll()
        {
            var query = new QueryDefinition($"SELECT * FROM c");
            return await GetFromQueryDefinition(query);
        }

        public async Task Add(T item)
        {
            var cosmosRecord = ToCosmosRecord(item);
            await _container.CreateItemAsync(cosmosRecord);
        }

        public async Task Update(string id, T item)
        {
            var cosmosRecord = ToCosmosRecord(item);
            await _container.ReplaceItemAsync(cosmosRecord, id, new PartitionKey("*"));
        }

        public async Task<List<T>> ExportToCsv()
        {
            var response = GetAll().Result;
                
                try
                {
                    if (response != null)
                    {
                        using (var writer = new StreamWriter("CsvExports/AllCampaigns.csv"))
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

    }
}
