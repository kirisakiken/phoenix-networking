using Newtonsoft.Json;

namespace KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes
{
    /// <summary>
    ///     Represents a TCP Payload
    /// </summary>
    public class TcpPayload : Payload
    {
        /// <summary>
        ///     Packet ID
        /// </summary>
        [JsonProperty("packetId")]
        public int PacketId { get; set; }

        /// <summary>
        ///     ID of Sender
        ///     -1 if Server
        /// </summary>
        [JsonProperty("senderId")]
        public int SenderId { get; set; }

        /// <summary>
        ///     Content message of the Payload
        /// </summary>
        [JsonProperty("tcpMessage")]
        public TcpMessage TcpMessage { get; set; } 
    }

    /// <summary>
    ///     Represents generic tcp message
    /// </summary>
    public class TcpMessage
    {
        /// <summary>
        ///     Id of receiver
        ///     0 if global
        /// </summary>
        [JsonProperty("receiverId")]
        public int ReceiverId { get; set; }

        /// <summary>
        ///     Content of message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}