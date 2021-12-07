using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Client : MonoBehaviour
    {
        #region PrivateFields

        private const int MAX_CONNECTION = 10;
        private const string DEFAULT_PLAYER_NAME = "Player";

        #endregion


        #region Fields

        public delegate void OnMessageReceiveDelegate(object message);
        public event OnMessageReceiveDelegate OnMessageReceive;

        [SerializeField] private bool _isConnected = false;
        [SerializeField] private bool _isActualConnected = false;
        [SerializeField] private string _playerName;

        private int _port = 0;
        private int _serverPort = 5805;
        private int _hostId;
        private int _reliableChannel;
        private int _connectionId;
        private byte _error;

        #endregion


        #region Properties

        public string PlayerName { get => _playerName; }

        internal void SetPlayerName(string e)
        {
            _playerName = e;
        }

        #endregion


        public void Connect()
        {
            NetworkTransport.Init();
            ConnectionConfig connectionConfig = new ConnectionConfig();
            _reliableChannel = connectionConfig.AddChannel(QosType.Reliable);

            HostTopology hostTopology = new HostTopology(connectionConfig, MAX_CONNECTION);

            _hostId = NetworkTransport.AddHost(hostTopology, _port);

            if (string.IsNullOrEmpty(_playerName))
            {
                _playerName = DEFAULT_PLAYER_NAME + _hostId;
            }

            _connectionId = NetworkTransport.Connect(_hostId, "127.0.0.1", _serverPort, 0, out _error);

            if ((NetworkError)_error == NetworkError.Ok)
            {
                _isConnected = true;
            }
            else
            {
                Debug.Log((NetworkError)_error);
            }
            StartCoroutine(SendPLayerNameToServer());
        }

        private IEnumerator SendPLayerNameToServer()
        {
            while (!_isActualConnected)
            {
                yield return new WaitForEndOfFrame();
            }
            var setPlayerName = MessageIdentifier.SetName + PlayerName;

            byte[] buffer = Encoding.Unicode.GetBytes(setPlayerName);

            NetworkTransport.Send(_hostId, _connectionId, _reliableChannel, buffer, setPlayerName.Length * sizeof(char), out _error);

            if ((NetworkError)_error != NetworkError.Ok)
            {
                Debug.Log((NetworkError)_error);
            }
        }

        public void Disconnect()
        {
            if (!_isConnected)
            {
                return;
            }
            NetworkTransport.Disconnect(_hostId, _connectionId, out _error);
            _isConnected = false;
            _isActualConnected = false;
        }


        private void Update()
        {
            if (!_isConnected)
            {
                return;
            }
            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;

            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out _error);

            while (recData != NetworkEventType.Nothing)
            {
                switch (recData)
                {
                    case NetworkEventType.Nothing:
                        break;

                    case NetworkEventType.ConnectEvent:
                        OnMessageReceive?.Invoke($"{PlayerName} You have been connected to server.");
                        Debug.Log($"{PlayerName} You have been connected to server.");
                        _isActualConnected = true;
                        break;

                    case NetworkEventType.DataEvent:
                        string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                        OnMessageReceive?.Invoke(message);
                        Debug.Log(message);
                        break;

                    case NetworkEventType.DisconnectEvent:
                        _isConnected = false;
                        _isActualConnected = false;
                        OnMessageReceive?.Invoke($"You have been disconnected from server.");
                        Debug.Log($"You have been disconnected from server.");
                        break;

                    case NetworkEventType.BroadcastEvent:

                        break;
                }

                recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out _error);
            }
        }

        public void SendMessageNetwork(string message)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);
            NetworkTransport.Send(_hostId, _connectionId, _reliableChannel, buffer, message.Length * sizeof(char), out _error);

            if ((NetworkError)_error != NetworkError.Ok)
            {
                Debug.Log((NetworkError)_error);
            }
        }
    }
}
