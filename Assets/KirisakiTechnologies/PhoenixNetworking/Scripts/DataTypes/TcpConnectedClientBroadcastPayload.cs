using Newtonsoft.Json;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes
{
    /// <summary>
    ///     Represents a broadcast payload when a client connects to server
    /// </summary>
    public class TcpConnectedClientBroadcastPayload : Payload
    {
        [JsonProperty("clientData")]
        public ClientData ClientData { get; set; }
    }
}