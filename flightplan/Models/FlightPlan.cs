using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FlightPlanApi.Models
{
    public class FlightPlan
    {
        public FlightPlan()
        {
            FlightPlanId = Guid.NewGuid().ToString("N");
            id = FlightPlanId;
        }

        [JsonProperty("flightPlanId")]
        public string FlightPlanId { get; }

        [JsonProperty("id")]
        public string id { get; }

        [JsonProperty("aircraftIdentification")]
        public string AircraftIdentification { get; set; }

        [JsonProperty("aircraftType")]
        public string AircraftType { get; set; }

        [JsonProperty("airspeed")]
        public int Airspeed { get; set; }

        [JsonProperty("altitude")]
        public int Altitude { get; set; }

        [JsonProperty("flightType")]
        public string FlightType { get; set; }

        [JsonProperty("fuelHours")]
        public int FuelHours { get; set; }

        [JsonProperty("fuelMinutes")]
        public int FuelMinutes { get; set; }

        [JsonProperty("departureTime")]
        public DateTime DepartureTime { get; set; }

        [JsonProperty("arrivalTime")]
        public DateTime ArrivalTime { get; set; }

        [JsonProperty("departingAirport")]
        public string DepartingAirport { get; set; }

        [JsonProperty("arrivalAirport")]
        public string ArrivalAirport { get; set; }

        [JsonProperty("route")]
        public string Route { get; set; }

        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        [JsonProperty("numberOnboard")]
        public int NumberOnBoard { get; set; }
    }
}
