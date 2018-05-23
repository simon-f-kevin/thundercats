using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;

namespace Game_Engine.Components.Preformance
{
    public class FPSComponent : Component
    {
        public float CurrentFramesPerSecond { get; set; } = 0;
        public float AverageFramesPerSecond { get; set; } = 0;
        public float TotalSeconds { get; set; }
        public float TotalFrames { get; set; }

        public FPSComponent(Entity id) : base(id)
        {
        }
    }
}
