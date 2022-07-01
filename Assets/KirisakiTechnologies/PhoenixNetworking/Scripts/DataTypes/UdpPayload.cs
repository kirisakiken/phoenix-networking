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
}