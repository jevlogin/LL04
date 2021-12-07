using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

namespace WORLDGAMEDEVELOPMENT
{
    public sealed class UIController : MonoBehaviour
    {
        [SerializeField] private Button _buttonStartServer;
        [SerializeField] private Button _buttonShutDownServer;
        [SerializeField] private Button _buttonConnectClient;
        [SerializeField] private Button _buttonDisconnectClient;
        [SerializeField] private Button _buttonSendMessage;
        [SerializeField] private Button _buttonQuit;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField, Tooltip("PlayerName")] private TMP_InputField _inputFieldPlayerName;
        [SerializeField] private MyTextField _textField;

        [SerializeField] private Server _server;
        [SerializeField] private Client _client;

        private void Start()
        {
            _inputFieldPlayerName.onEndEdit.AddListener((e) =>
            {
                _client.SetPlayerName(e);
            });
            _buttonStartServer.onClick.AddListener(StartServer);
            _buttonShutDownServer.onClick.AddListener(ShutDownServer);
            _buttonConnectClient.onClick.AddListener(Connect);
            _buttonDisconnectClient.onClick.AddListener(Disconnect);
            _buttonSendMessage.onClick.AddListener(SendMessage);
            _client.OnMessageReceive += ReceiveMessage;
            _buttonQuit.onClick.AddListener(() => Application.Quit());
        }


        private void OnDestroy()
        {
            _inputFieldPlayerName.onEndEdit.RemoveAllListeners();
            _client.OnMessageReceive -= ReceiveMessage;
            _buttonStartServer.onClick.RemoveAllListeners();
            _buttonShutDownServer.onClick.RemoveAllListeners();
            _buttonConnectClient.onClick.RemoveAllListeners();
            _buttonDisconnectClient.onClick.RemoveAllListeners();
            _buttonSendMessage.onClick.RemoveAllListeners();
            _buttonQuit.onClick.RemoveAllListeners();
        }

        private void ReceiveMessage(object message)
        {
            _textField.ReceiveMessage(message);
        }

        private void SendMessage()
        {
            _client.SendMessageNetwork(_inputField.text);
            _inputField.text = string.Empty;
        }

        private void Disconnect()
        {
            _client.Disconnect();
        }

        private void Connect()
        {
            _client.Connect();
        }

        private void ShutDownServer()
        {
            _server.ShutDownServer();
        }

        private void StartServer()
        {
            _server.StartServer();
        }
    }
}
