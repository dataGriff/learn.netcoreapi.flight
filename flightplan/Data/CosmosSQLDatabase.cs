using FlightPlanApi.Models;
using Microsoft.Azure.Cosmos;

namespace FlightPlanApi.Data
{
    public class CosmosSQLDatabase : IDatabaseAdapter
    {
        public CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;

        public CosmosSQLDatabase()
        {
            string endpoint = Environment.GetEnvironmentVariable("COSMOS_ENDPOINT");
            string key = Environment.GetEnvironmentVariable("COSMOS_KEY");

            // Initialize CosmosClient
            _cosmosClient = new CosmosClient(endpoint, key);

            // Call the async initialization method.
            InitializeAsync().GetAwaiter().GetResult();
        }

        private async Task InitializeAsync()
        {
            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync("pluralsight");
            _container = await _database.CreateContainerIfNotExistsAsync("flight_plans", "/flightPlanId");
        }

        public async Task<List<FlightPlan>> GetAllFlightPlans()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            FeedIterator<FlightPlan> resultSet = _container.GetItemQueryIterator<FlightPlan>(query);

            List<FlightPlan> results = new List<FlightPlan>();
            
            while (resultSet.HasMoreResults)
            {
                FeedResponse<FlightPlan> response = await resultSet.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<FlightPlan> GetFlightPlanById(string flightPlanId)
        {
            try
            {

                ItemResponse<FlightPlan> response = await _container.ReadItemAsync<FlightPlan>(flightPlanId, new PartitionKey(flightPlanId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<TransactionResult> FileFlightPlan(FlightPlan flightPlan)
        {
            try
            {
                Console.WriteLine("Creating new item in database...");
                Console.WriteLine(flightPlan.FlightPlanId);
                ItemResponse<FlightPlan> response = await _container.CreateItemAsync<FlightPlan>(flightPlan);

                return TransactionResult.Success;
            }
            catch (Exception)
            {
                return TransactionResult.ServerError;
            }
        }

        public async Task<bool> DeleteFlightPlanById(string flightPlanId)
        {
            try
            {
                ItemResponse<FlightPlan> response = await _container.DeleteItemAsync<FlightPlan>(flightPlanId, new PartitionKey(flightPlanId));
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<TransactionResult> UpdateFlightPlan(string flightPlanId, FlightPlan flightPlan)
        {
            try
            {
                ItemResponse<FlightPlan> response = await _container.ReplaceItemAsync(flightPlan, flightPlanId, new PartitionKey(flightPlanId));
                return TransactionResult.Success;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return TransactionResult.NotFound;
            }
            catch (Exception)
            {
                return TransactionResult.ServerError;
            }
        }
    }
}
