using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using KirisakiTechnologies.GameSystem.Scripts;
using KirisakiTechnologies.GameSystem.Scripts.Extensions;
using KirisakiTechnologies.GameSystem.Scripts.Tools;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Modules;
using KirisakiTechnologies.PhoenixNetworking.Scripts.Client.Providers;
using KirisakiTechnologies.PhoenixNetworking.Scripts.DataTypes;
using MixChatter.Scripts.Components;
using UnityEngine;
using UnityEngine.UI;

namespace MixChatter.Scripts.Tools
{
    public class NetworkChatTool : ToolMonoBase
    {
        #region Overrides

        public override Task Initialize(IGameSystem gameSystem)
        {
            _ClientModule = gameSystem.GetModule<IClientModule>();
            _TcpPacketProvider = gameSystem.GetProvider<ITcpPacketProvider>();
            _NetworkEventHandlerModule = gameSystem.GetModule<INetworkEventHandlerModule>();

            _NetworkEventLogicModule = gameSystem.GetModule<INetworkEventLogicModule>();
            _NetworkEventLogicModule.OnTcpInitialConnectPayloadReceived += TcpInitialConnectPayloadReceivedHandler;
            _NetworkEventLogicModule.OnTcpConnectedClientBroadcastPayloadReceived += TcpConnectedClientBroadcastPayloadReceivedHandler;
            _NetworkEventLogicModule.OnTcpClientMessagePayloadReceived += TcpClientMessagePayloadReceivedHandler;
            _NetworkEventLogicModule.OnHandshakePacketRequested += HandshakePacketRequestedHandler;

            return base.Initialize(gameSystem);
        }

        #endregion

        #region Public

        [UsedImplicitly]
        public void OnConnectButtonClicked()
        {
            var nickName = _NameInputField.text;
            var ipPort = _IpInputField.text.Split(':');
            var ip = ipPort[0];
            var portStr = ipPort[1];

            if (!int.TryParse(portStr, out var port))
                throw new InvalidOperationException($"Unable to parse port from: {portStr}");

            _ClientModule.ConnectToServer(nickName, ip, (uint) port);
        }

        [UsedImplicitly]
        public void OnSendButtonClicked()
        {
            _NetworkEventHandlerModule.SendTcpClientMessageToServer(_ClientModule.Id, _MessageInputField.text);
            _MessageInputField.text = "Enter text...";
        }

        [UsedImplicitly]
        public void OnDisconnectButtonClicked()
        {
            // switch to main menu
            _LoginScreen.SetActive(true);
            _ChatScreen.SetActive(false);

            // disconnect from server
            _ClientModule.DisconnectFromServer();

            // destroy chat/user components
            var chatComponents = FindObjectsOfType<ChatComponent>();
            for (var i = chatComponents.Length - 1; i >= 0; --i)
                Destroy(chatComponents[i].gameObject);

            var userComponents = FindObjectsOfType<UserComponent>();
            for (var i = userComponents.Length - 1; i >= 0; --i)
                Destroy(userComponents[i].gameObject);
        }

        #endregion

        #region Event Handlers

        private void TcpInitialConnectPayloadReceivedHandler(TcpInitialConnectPayload payload)
        {
            // Instantiate available clients
            foreach (var clientData in payload.AvailableClients)
            {
                var go = Instantiate(_ClientPrefab, _ClientsView, false);
                if (!go.TryGetComponent(out UserComponent userComponent))
                    throw new NullReferenceException($"Unable to find component of type {nameof(UserComponent)}");

                userComponent.UsernameText.text = clientData.ClientName;
            }
        }

        private void TcpConnectedClientBroadcastPayloadReceivedHandler(TcpConnectedClientBroadcastPayload payload)
        {
            // Instantiate connected client
            var go = Instantiate(_ClientPrefab, _ClientsView, false);
            if (!go.TryGetComponent(out UserComponent userComponent))
                throw new NullReferenceException($"Unable to find component of type {nameof(UserComponent)}");

            userComponent.UsernameText.text = payload.ClientData.ClientName;
        }

        private void TcpClientMessagePayloadReceivedHandler(TcpClientMessagePayload payload)
        {
            // Instantiate received message in chat
            var go = Instantiate(_ChatPrefab, _ChatsView, false);
            if (!go.TryGetComponent(out ChatComponent chatComponent))
                throw new NullReferenceException($"Unable to find component of type {nameof(ChatComponent)}");

            chatComponent.UsernameText.text = payload.ClientData.ClientName;
            chatComponent.MessageText.text = payload.Message;
        }

        private void HandshakePacketRequestedHandler(int clientId)
        {
            _LoginScreen.SetActive(false);
            _ChatScreen.SetActive(true);

            using (var welcomeReceivedPacket = _TcpPacketProvider.OnConnectWelcomeReceivedPacket(clientId, _NameInputField.text))
                _NetworkEventHandlerModule.SendTcpDataToServer(welcomeReceivedPacket);
        }

        #endregion

        #region Private

        [SerializeField]
        private GameObject _LoginScreen;

        [SerializeField]
        private GameObject _ChatScreen;

        [SerializeField]
        private InputField _NameInputField;

        [SerializeField]
        private InputField _IpInputField;

        [SerializeField]
        private RectTransform _ClientsView;

        [SerializeField]
        private RectTransform _ChatsView;

        [SerializeField]
        private GameObject _ClientPrefab;

        [SerializeField]
        private GameObject _ChatPrefab;

        [SerializeField]
        private InputField _MessageInputField;

        private IClientModule _ClientModule;
        private ITcpPacketProvider _TcpPacketProvider;
        private INetworkEventHandlerModule _NetworkEventHandlerModule;
        private INetworkEventLogicModule _NetworkEventLogicModule;

        #endregion
    }
}
