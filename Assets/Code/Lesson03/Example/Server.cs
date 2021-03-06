using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Server : MonoBehaviour
    {
        #region PrivateFields
        
        private const int MAX_CONNECTION = 10;

        #endregion

        
        #region Fields
        
        [SerializeField] private List<int> _connectionIDs = new List<int>();
        [SerializeField] private Dictionary<int, string> _playerNameIds = new Dictionary<int, string>();

        private int _port = 5805;
        private int _hostId;
        private int _reliableChannel;
        private bool _isStarted = false;
        private byte _error;

        #endregion

        
        #region ClassLifeCycles
        
        private void OnDestroy()
        {
            _connectionIDs.Clear();
            _playerNameIds.Clear();
        }

        #endregion


        #region UnityMethods

        private void Update()
        {
            if (!_isStarted)
            {
                return;
            }

            LogicServer();
        }

        #endregion


        #region Methods

        public void StartServer()
        {
            NetworkTransport.Init();
            ConnectionConfig connectionConfig = new ConnectionConfig();
            _reliableChannel = connectionConfig.AddChannel(QosType.Reliable);
            HostTopology hostTopology = new HostTopology(connectionConfig, MAX_CONNECTION);
            _hostId = NetworkTransport.AddHost(hostTopology, _port);
            _isStarted = true;
            Debug.Log($"Server started");
        }

        public void ShutDownServer()
        {
            if (!_isStarted)
            {
                return;
            }
            NetworkTransport.RemoveHost(_hostId);
            NetworkTransport.Shutdown();
            _isStarted = false;
        }

        public void SendMessage(string message, int connectionId)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);
            NetworkTransport.Send(_hostId, connectionId, _reliableChannel, buffer, message.Length * sizeof(char), out _error);
            if ((NetworkError)_error != NetworkError.Ok)
            {
                Debug.Log((NetworkError)_error);
            }
        }

        public void SendMessageToAll(string message)
        {
            for (int i = 0; i < _connectionIDs.Count; i++)
            {
                SendMessage(message, _connectionIDs[i]);
            }
        }

        private void LogicServer()
        {
            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 10224;
            int dataSize;

            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out _error);

            while (recData != NetworkEventType.Nothing)
            {
                switch (recData)
                {
                    case NetworkEventType.DataEvent:
                        string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                        string ident = string.Empty;
                        for (int i = 0; i < 4; i++)
                        {
                            ident += message[i];
                        }
                        var name = message.Trim(ident.ToCharArray());

                        if (ident.Equals(MessageIdentifier.SetName))
                        {
                            if (_playerNameIds.ContainsKey(connectionId))
                            {
                                _playerNameIds[connectionId] = name;
                                SendMessageToAll($"name = {name}");
                                SendMessageToAll($"_playerNameIds = {_playerNameIds[connectionId]}");
                            }
                            else
                            {
                                _playerNameIds.Add(connectionId, name);
                            }
                        }
                        else
                        {
                            SendMessageToAll($"{_playerNameIds[connectionId]}: { message}");
                        }
                        Debug.Log($"{_playerNameIds[connectionId]}: {message}");

                        break;

                    case NetworkEventType.ConnectEvent:
                        _connectionIDs.Add(connectionId);

                        if (_playerNameIds.ContainsKey(connectionId))
                        {
                            SendMessageToAll($"{_playerNameIds[connectionId]} has connected");
                            Debug.Log($"{_playerNameIds[connectionId]} has connected");
                        }
                        else
                        {
                            SendMessageToAll($"Player {connectionId} has connected");
                            Debug.Log($"Player {connectionId} has connected");
                        }
                        break;

                    case NetworkEventType.DisconnectEvent:
                        _connectionIDs.Remove(connectionId);
                        SendMessageToAll($"{_playerNameIds[connectionId]} has disconnected");
                        Debug.Log($"{_playerNameIds[connectionId]} has disconnected");
                        break;

                    case NetworkEventType.Nothing:
                        break;
                    case NetworkEventType.BroadcastEvent:
                        break;
                }
                recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out _error);
            }
        }

        #endregion
    }
}
