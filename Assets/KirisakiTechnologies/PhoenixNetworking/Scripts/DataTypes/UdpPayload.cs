using KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules;
using Newtonsoft.Json;

using UnityEngine;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes
{
    // TODO: add description
    public class UdpPayload : Payload
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    // TODO: add description
    public class ClientUdpPayload : Payload
    {
        // TODO: have key codes and states (true/false)
    }
}