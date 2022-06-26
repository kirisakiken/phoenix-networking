using Newtonsoft.Json;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes
{
    /// <summary>
    ///     Represents a message payload sent by client
    /// </summary>
    public class TcpClientMessagePayload : Payload
    {
        [JsonProperty("clientData")]
        public ClientData ClientData { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}