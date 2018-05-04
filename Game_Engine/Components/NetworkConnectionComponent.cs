using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;

namespace Game_Engine.Components
{
    public class NetworkConnectionComponent : Component
    {
        public string Hostname { get; set; }

        public int Port { get; set; }


        public NetworkConnectionComponent(Entity id) : base(id)
        {

        }
    }
}
