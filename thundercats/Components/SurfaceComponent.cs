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
        public enum Surface { Fast = -20, Standard = 0, Slow = 80 };
        public Surface SurfaceType { get; set; }
        public SurfaceComponent(Entity id, Surface surfaceType) : base(id)
        {
            SurfaceType = surfaceType;
        }
    }
}
