using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Helpers
{
    public class NetworkHelper
    {
        /// <summary>
        /// This is the port where the game connection will be played on. 
        /// This may change its implementation in a later stage.
        /// </summary>
        private static int Port = 14242;

        public enum ConnectionType
        {
            Host,
            Client,
        }

        /// <summary>
        /// Returns the port
        /// </summary>
        /// <returns></returns>
        public static int GetPort()
        {
            return Port;
        }

        /// <summary>
        /// Returns the Ipv4-address of the current PC in the current LAN
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentIPAddress()
        {
            var hostname = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(hostname);
            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length-1].ToString(); //This is the Ipv4-address
        }
    }
}
