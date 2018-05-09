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

        private NetPeerConfiguration netPeerConfiguration;

        private NetworkConnectionComponent networkConnectionComponent;

        private NetServer server;

        private NetClient client;


        public NetworkConnectionManager(NetworkHelper.ConnectionType type)
        {
            if (type == NetworkHelper.ConnectionType.Host)
            {
                InitConnectionManagerAsServer();
            }

            if (type == NetworkHelper.ConnectionType.Client)
            {
                InitConnectionManagerAsClient();
            }
        }

        /// <summary>
        /// This method starts a server instance on this PC 
        /// </summary>
        public void StartServer()
        {
            ServerName = networkConnectionComponent.Hostname;
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
        public NetServer GetServer()
        {
            if(server.Status == NetPeerStatus.Running) return server;
            return null;
        }

        public void ClientSearch()
        {
            client.Start();
            // Emit a discovery signal
            client.DiscoverLocalPeers(netPeerConfiguration.Port);
           
            NetIncomingMessage inc;
            while ((inc = client.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        var endpoint = inc.SenderEndPoint;
                        var name = inc.ReadString();
                        Console.WriteLine("Found server at " + endpoint + " name: " + name);
                        SetServerName(name);
                        break;
                }
            }
            
        }

        private void SetServerName(string name)
        {
            ServerName = name;
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
                new NetPeerConfiguration(networkConnectionComponent.Hostname) {Port = networkConnectionComponent.Port};

        }

    }
}
