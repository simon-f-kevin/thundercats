using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Game_Engine.Components;
using Game_Engine.Helpers;

namespace Game_Engine.Managers
{
    public class NetworkConnectionManager
    {
        public string ServerName = "";

        private NetPeerConfiguration netPeerConfiguration;

        private NetworkConnectionComponent networkConnectionComponent;

        private NetServer Server;

        private NetClient Client;

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
            Server.Start();
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
            if (Server != null)
            {
                Server.Shutdown("bye!");
            }
        }


        private void InitConnectionManagerAsServer()
        {
            InitConnectionManager();
            Server = new NetServer(netPeerConfiguration);
        }

        private void InitConnectionManagerAsClient()
        {
            InitConnectionManager();
            Client = new NetClient(netPeerConfiguration);
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
