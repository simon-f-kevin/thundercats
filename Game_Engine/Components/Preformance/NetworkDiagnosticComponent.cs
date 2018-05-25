using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;

namespace Game_Engine.Components.Preformance
{
    public class NetworkDiagnosticComponent : Component
    {
        public int TotalDataSent { get; set; }
        public int DataSentThisSecond { get; set; }
        public float Ping { get; set; }
        public double PacketLoss { get; set; }

        public NetworkDiagnosticComponent(Entity id) : base(id)
        {
        }
    }
}
