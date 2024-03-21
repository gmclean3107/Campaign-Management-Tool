using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Drawing.Text;
using System.Formats.Asn1;
using CsvHelper;
using Azure;
using System.Globalization;
using System.Text;

namespace CampaignManagementTool.Server.Repositories
{
    /// <summary>
    /// Represents a record in Cosmos DB.
    /// </summary>
    /// <typeparam name="T">The type of payload.</typeparam
    public class CosmosRecord<T>
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty(PropertyName = "payload")]
        public T Payload { get; set; }
    }

    /// <summary>
    /// Base repository for interacting with Cosmos DB.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public abstract class CosmosDbRepository<T> where T : class
    {
        private readonly string _databaseId;
        private readonly string _containerId;
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        /// <summary>
        /// Converts an entity payload to a Cosmos record.
        /// </summary>
        /// <param name="payload">The payload to convert.</param>
        /// <returns>The Cosmos record.</returns>
        protected virtual CosmosRecord<T> ToCosmosRecord(T payload) => throw new NotImplementedException();

        /// <summary>
        /// Constructs a new instance of <see cref="CosmosDbRepository{T}"/>.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="databaseId">The ID of the database.</param>
        /// <param name="containerId">The ID of the container.</param>
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

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity, or <c>null</c> if not found.</returns>
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

        /// <summary>
        /// Retrieves entities based on a query definition.
        /// </summary>
        /// <param name="queryDefinition">The query definition.</param>
        /// <returns>The list of entities matching the query.</returns>
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

        /// <summary>
        /// Retrieves all entities.
        /// </summary>
        /// <returns>The list of entities.</returns>
        public async Task<List<T>> GetAll()
        {
            var query = new QueryDefinition($"SELECT * FROM c");
            return await GetFromQueryDefinition(query);
        }

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="item">The entity to add.</param>
        public async Task Add(T item)
        {
            var cosmosRecord = ToCosmosRecord(item);
            await _container.CreateItemAsync(cosmosRecord);
        }

        /// <summary>
        /// Updates an entity in the repository.
        /// </summary>
        /// <param name="id">The ID of the entity to update.</param>
        /// <param name="item">The updated entity.</param>
        public async Task Update(string id, T item)
        {
            var cosmosRecord = ToCosmosRecord(item);
            await _container.ReplaceItemAsync(cosmosRecord, id, new PartitionKey("*"));
        }

        /// <summary>
        /// Exports all entities to a CSV byte array.
        /// </summary>
        /// <returns>The CSV byte array.</returns>
        public async Task<byte[]> ExportToCsv()
        {
            var response = await GetAll();

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

    }
}
