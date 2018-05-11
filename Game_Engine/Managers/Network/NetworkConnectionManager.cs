using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Game_Engine.Components;
using Game_Engine.Helpers;

namespace Game_Engine.Managers.Network
{
    public class NetworkConnectionManager
    {
        public string ServerName = "";

        public bool IsHost;

        private NetPeerConfiguration netPeerConfiguration;

        private NetworkConnectionComponent networkConnectionComponent;

        private NetServer server;

        private NetClient client;


        public NetworkConnectionManager(NetworkHelper.ConnectionType type)
        {
            if (type == NetworkHelper.ConnectionType.Host)
            {
                InitConnectionManagerAsServer();
                IsHost = true;
            }

            if (type == NetworkHelper.ConnectionType.Client)
            {
                InitConnectionManagerAsClient();
                IsHost = false;
            }
        }

        /// <summary>
        /// This method starts a server instance on this PC 
        /// </summary>
        public void StartServer()
        {
            ServerName = networkConnectionComponent.IPAddress;
            server.Start();
        }

        /// <summary>
        /// Shuts down the current server instance
        /// </summary>
        public void ExitServer()
        {
            //can use null propagation like this: (which is harder to read)
            /*
             * Server?.Shutdown("bye!");
             */
            if (server != null)
            {
                server.Shutdown("bye!");
            }
        }

        /// <summary>
        /// Returns the server if it is running
        /// </summary>
        /// <returns></returns>
        public NetPeer GetPeer()
        {
            if(server != null && server.Status == NetPeerStatus.Running) return server;
            if (client != null && client.Status == NetPeerStatus.Running) return client;
            return null;
        }
        /// <summary>
        /// This method starts the client and searches for a server in the LAN
        /// </summary>
        public void ClientSearch()
        {
            client.Start();

            client.DiscoverLocalPeers(netPeerConfiguration.Port);

            while (true)
            {
                NetIncomingMessage inc = client.ReadMessage();
                inc = client.WaitMessage(500);
                if (inc == null) break;
                if (client.Connections.Count > 0)
                {
                    break;
                }
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        var endpoint = inc.SenderEndPoint;
                        var name = inc.ReadString();
                        Console.WriteLine("Found server at " + endpoint + " name: " + name);
                        ServerName = name;
                        client.Connect(inc.SenderEndPoint.Address.ToString(), inc.SenderEndPoint.Port);
                        break;
                }
            }

        }

        private void InitConnectionManagerAsServer()
        {
            InitConnectionManager();
            server = new NetServer(netPeerConfiguration);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
        }

        private void InitConnectionManagerAsClient()
        {
            InitConnectionManager();
            client = new NetClient(netPeerConfiguration);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
        }

        private void InitConnectionManager()
        {
            networkConnectionComponent = ComponentManager.Instance.GetDictionary<NetworkConnectionComponent>()
                .FirstOrDefault().Value as NetworkConnectionComponent;
            if (networkConnectionComponent == null) return;
            netPeerConfiguration =
                new NetPeerConfiguration("thundercats") {Port = networkConnectionComponent.Port};

        }

    }
}
