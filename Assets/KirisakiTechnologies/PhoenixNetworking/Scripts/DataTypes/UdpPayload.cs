using System.Collections.Generic;

using Newtonsoft.Json;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes
{
    // TODO: add description
    public class UdpPayload : Payload
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    // TODO: RESEARCH AND REDESIGN BELOW
    // Question?: how different type of entities will be deserialized and consumed by entities architecture?
    //
    // 1- one solution might be storing type and object data e.g.
    //
    // EntityType type;
    // object data;
    //
    // deserializing -> check entity type, and deserialize data according to type
    // serializing -> check entity interface, and serialize it according to interface
    // ---------------------
    // 2- Other approach would be the having AddedEntities defined as class
    // but it won't be generic
    // ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    // Going with approach 1
    public class UdpServerTickPayload : Payload
    {
        [JsonProperty("added_entities")]
        public List<GenericNetworkEntity> AddedEntities { get; set; }

        [JsonProperty("modified_entities")]
        public List<GenericNetworkEntity> ModifiedEntities { get; set; }

        [JsonProperty("removed_entities")]
        public List<GenericNetworkEntity> RemovedEntities { get; set; }
    }

    public class UdpClientInputPayload : Payload
    {
        [JsonProperty("modified_keys")]
        public Dictionary<ClientInputKey, bool> ModifiedKeys { get; set; }
    }

    public class GenericNetworkEntity
    {
        [JsonProperty("entity_type")]
        public EntityType EntityType { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public class PlayerNetworkEntity
    {
        [JsonProperty("entity_id")]
        public int EntityId { get; set; }

        [JsonProperty("client_id")]
        public int ClientId { get; set; }

        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [JsonProperty("network_id")]
        public string NetworkId { get; set; }

        [JsonProperty("position")]
        public Point3D Position { get; set; }

        [JsonProperty("rotation")]
        public Point4D Rotation { get; set; }
    }

    public struct Point3D
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }

        [JsonProperty("z")]
        public float Z { get; set; }
    }

    public struct Point4D
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }

        [JsonProperty("z")]
        public float Z { get; set; }

        [JsonProperty("w")]
        public float W { get; set; }
    }
}