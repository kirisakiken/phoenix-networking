using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes
{
    /// <summary>
    ///     Initial TCP connection payload that needs to be sent to
    ///     connected client on initial connection
    /// </summary>
    public class TcpInitialConnectPayload : Payload
    {
        /// <summary>
        ///     Represents the client ID assigned by the server
        /// </summary>
        [JsonProperty("clientId")]
        public int ClientId { get; set; }

        /// <summary>
        ///     Represents available clients on server side
        /// </summary>
        [JsonProperty("availableClients")]
        public List<ClientData> AvailableClients { get; set; }
    }
}