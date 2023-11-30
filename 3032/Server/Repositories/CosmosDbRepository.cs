﻿using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace CampaignManagementTool.Server.Repositories
{
    public class CosmosRecord<T>
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "partitionKey")]
        public string paritionKey { get; set; }

        [JsonProperty(PropertyName = "payload")]
        public T Payload { get; set; }
    }

    public class CosmosDbRepository<T> where T : class
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

        public async Task<List<T>> GetAll()
        {
            var query = new QueryDefinition($"SELECT * FROM c");
            var iterator = _container.GetItemQueryIterator<CosmosRecord<T>>(query);

            var results = new List<T>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.Resource.Select(r => r.Payload));
            }

            return results;
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

    }
}
