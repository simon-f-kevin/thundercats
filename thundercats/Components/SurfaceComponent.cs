using System;
using System.Collections.Generic;
using Game_Engine.Components;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;

namespace thundercats.Components
{
    public class SurfaceComponent : Component
    {
        public float SpeedPenalty { get; set;}
        public SurfaceComponent(Entity id, float speedPenalty) : base(id)
        {
            SpeedPenalty = speedPenalty;
        }
    }
}
