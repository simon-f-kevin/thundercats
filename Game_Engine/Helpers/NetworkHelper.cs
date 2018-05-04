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
        private static int Port = 14242;

        public enum ConnectionType
        {
            Host,
            Client,
        }

        public static int GetPort()
        {
            return Port;
        }

        public static string GetCurrentHostname()
        {
            return Dns.GetHostName();
        }
    }
}
