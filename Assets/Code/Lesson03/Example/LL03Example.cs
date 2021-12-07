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
    public sealed class LL03Example : MonoBehaviour
    {
        private byte error;
        private byte[] buffer;
        private int bufferLength;
        private int recHostId;
        private int channelId;
        private byte[] recBuffer = new byte[1024];
        private int bufferSize = 1024;
        private int dataSize;
        private int connectionId;

        private void Start()
        {
            GlobalConfig gConfig = new GlobalConfig()
            {
                MaxPacketSize = 500
            };
            NetworkTransport.Init(gConfig);

            ConnectionConfig config = new ConnectionConfig();
            int myReliableChannelId = config.AddChannel(QosType.Reliable);
            int myUnReliableChannelId = config.AddChannel(QosType.Unreliable);

            HostTopology hostTopology = new HostTopology(config, 10);
            int hostId = NetworkTransport.AddHost(hostTopology, 8888);
            int hostIdSocket = NetworkTransport.AddWebsocketHost(hostTopology, 8887, null);

            var connectionId = NetworkTransport.Connect(hostId, "127.0.0.1", 8888, 0, out error);

            NetworkTransport.Send(hostId, connectionId, myReliableChannelId, buffer, bufferLength, out error);
            NetworkTransport.Disconnect(hostId, connectionId, out error);

            if ((NetworkError)error != NetworkError.Ok)
            {
                Debug.Log((NetworkError)error);
            }

            NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
            NetworkTransport.ReceiveFromHost(recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

        }

        private void Update()
        {
            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

            switch (recData)
            {
                case NetworkEventType.DataEvent:
                    break;
                case NetworkEventType.ConnectEvent:
                    break;
                case NetworkEventType.DisconnectEvent:
                    break;
                case NetworkEventType.Nothing:
                    break;
                case NetworkEventType.BroadcastEvent:
                    break;
                default:
                    break;
            }
        }

    }
}
