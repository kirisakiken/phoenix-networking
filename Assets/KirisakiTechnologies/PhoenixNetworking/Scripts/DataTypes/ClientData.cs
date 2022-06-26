using Newtonsoft.Json;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes
{
    /// <summary>
    ///     Represents a client data structure
    /// </summary>
    public class ClientData
    {
        /// <summary>
        ///     ID of the client
        /// </summary>
        [JsonProperty("clientId")]
        public int ClientId { get; set; }

        /// <summary>
        ///     Name of the client
        /// </summary>
        [JsonProperty("clientName")]
        public string ClientName { get; set; }
    }
}