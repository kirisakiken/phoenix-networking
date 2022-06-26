﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes
{
    /// <summary>
    ///     Initial TCP connection payload that needs to be sent to
    ///     connected client on initial connection
    /// </summary>
    public class TcpInitialConnectPayload
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