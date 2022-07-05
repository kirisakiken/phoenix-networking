using System.Collections.Generic;

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
        public Dictionary<KeyCode, bool> ModifiedKeys { get; set; } // TODO: replace KeyCode with internally defined key code enum
    }

    public class GenericNetworkEntity
    {
        [JsonProperty("entity_type")]
        public EntityType EntityType { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}