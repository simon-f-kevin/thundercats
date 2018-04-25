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
        public enum Surface{Ice = -20, Standard = 0, Mud = 80};
        public SurfaceComponent(Entity id) : base(id)
        {

        }
    }
}
