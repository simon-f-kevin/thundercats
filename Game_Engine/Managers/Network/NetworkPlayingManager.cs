using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Game_Engine.Managers.Network
{
    public class NetworkPlayingManager
    {
        private NetworkConnectionManager connectionManager;

        public NetworkPlayingManager(NetworkConnectionManager connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        public void RecieveMessage()
        {

        }


    }
}
