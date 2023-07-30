using MongoDB.Bson;
using MongoDB.Driver;
using FlightPlanApi.Models;

namespace FlightPlanApi.Data
{
    public class MongoDbDatabase : IDatabaseAdapter
    {
        public async Task<List<FlightPlan>> GetAllFlightPlans()
        {
            var collection = GetCollection("pluralsight", "flight_plans");
            var documents = collection.Find(_ => true).ToListAsync();

            var flightPlanList = new List<FlightPlan>();

            if (documents == null) return flightPlanList;

            foreach (var document in await documents)
            {
                flightPlanList.Add(ConvertBsonToFlightPlan(document));
            }

            return flightPlanList;
        }

        public async Task<FlightPlan> GetFlightPlanById(string flightPlanId)
        {
            var collection = GetCollection("pluralsight", "flight_plans");
            var flightPlanCursor = await collection.FindAsync(
                Builders<BsonDocument>.Filter.Eq("flightPlanId", flightPlanId));
            var document = flightPlanCursor.FirstOrDefault();
            var flightPlan = ConvertBsonToFlightPlan(document);

            if (flightPlan == null)
            {
                return new FlightPlan();
            }

            return flightPlan;
        }

        public async Task<TransactionResult> FileFlightPlan(FlightPlan flightPlan)
        {
            var collection = GetCollection("pluralsight", "flight_plans");

            var document = new BsonDocument
            {
                {"flightPlanId", flightPlan.FlightPlanId },
                {"altitude", flightPlan.Altitude },
                {"airspeed", flightPlan.Airspeed },
                {"aircraftIdentification", flightPlan.AircraftIdentification },
                {"aircraftType", flightPlan.AircraftType },
                {"arrivalAirport", flightPlan.ArrivalAirport },
                {"flightType", flightPlan.FlightType },
                {"departingAirport", flightPlan.DepartingAirport },
                {"departureTime", flightPlan.DepartureTime },
                {"estimatedArrivalTime", flightPlan.ArrivalTime },
                {"route", flightPlan.Route },
                {"remarks", flightPlan.Remarks },
                {"fuelHours", flightPlan.FuelHours },
                {"fuelMinutes", flightPlan.FuelMinutes },
                {"numberOnboard", flightPlan.NumberOnBoard }
            };

            // try
            // {
                await collection.InsertOneAsync(document);
                if (document["_id"].IsObjectId)
                {
                    return TransactionResult.Success; 
                }

                return TransactionResult.BadRequest;
            // }
            // catch
            // {
            //     return TransactionResult.ServerError;
            // }
                        
        }

        public async Task<bool> DeleteFlightPlanById(string flightPlanId)
        {
            var collection = GetCollection("pluralsight", "flight_plans");
            var result = await collection.DeleteOneAsync(
                Builders<BsonDocument>.Filter.Eq("flightPlanId", flightPlanId));

            return result.DeletedCount > 0;
        }

        public async Task<TransactionResult> UpdateFlightPlan(string flightPlanId, FlightPlan flightPlan)
        {
            var collection = GetCollection("pluralsight", "flight_plans");
            var filter = Builders<BsonDocument>.Filter.Eq("flightPlanId", flightPlanId);
            var update = Builders<BsonDocument>.Update
                .Set("altitude", flightPlan.Altitude)
                .Set("airspeed", flightPlan.Airspeed)
                .Set("aircraftIdentification", flightPlan.AircraftIdentification)
                .Set("aircraftType", flightPlan.AircraftType)
                .Set("arrivalAirport", flightPlan.ArrivalAirport)
                .Set("flightType", flightPlan.FlightType)
                .Set("departingAirport", flightPlan.DepartingAirport)
                .Set("departureTime", flightPlan.DepartureTime)
                .Set("estimatedArrivalTime", flightPlan.ArrivalTime)
                .Set("route", flightPlan.Route)
                .Set("remarks", flightPlan.Remarks)
                .Set("fuelHours", flightPlan.FuelHours)
                .Set("fuelMinutes", flightPlan.FuelMinutes)
                .Set("numberOnBoard", flightPlan.NumberOnBoard);
            var result = await collection.UpdateOneAsync(filter, update);

            if(result.MatchedCount == 0)
            {
                return TransactionResult.NotFound;
            }

            if(result.ModifiedCount > 0)
            {
                return TransactionResult.Success;
            }

            return TransactionResult.ServerError;
            
        }

        private IMongoCollection<BsonDocument> GetCollection(
            string databaseName, string collectionName)
        {
            var client = new MongoClient("mongodb://lrn-data-cosdbmon-eun-dgrf:ChDNb3gah6vV4T3B3kJWMDzhd0uuEpuZ1Wbg1YsYLbtwdxQkze67huOwjVqVmH5DywhwIorODMs7ACDbp6KvOA==@lrn-data-cosdbmon-eun-dgrf.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@lrn-data-cosdbmon-eun-dgrf@");
            var database = client.GetDatabase(databaseName);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            return collection;
        }

        private FlightPlan ConvertBsonToFlightPlan(BsonDocument document)
        {
            if (document == null) return null;

            return new FlightPlan
            {
                //FlightPlanId = document["flightPlanId"].AsString,
                Altitude = document["altitude"].AsInt32,
                Airspeed = document["airspeed"].AsInt32,
                AircraftIdentification = document["aircraftIdentification"].AsString,
                AircraftType = document["aircraftType"].AsString,
                ArrivalAirport = document["arrivalAirport"].AsString,
                FlightType = document["flightType"].AsString,
                DepartingAirport = document["departingAirport"].AsString,
                DepartureTime = document["departureTime"].AsBsonDateTime.ToUniversalTime(),
                ArrivalTime = document["estimatedArrivalTime"].AsBsonDateTime.ToUniversalTime(),
                Route = document["route"].AsString,
                Remarks = document["remarks"].AsString,
                FuelHours = document["fuelHours"].AsInt32,
                FuelMinutes = document["fuelMinutes"].AsInt32,
                NumberOnBoard = document["numberOnboard"].AsInt32
            };
        }
    }
}
