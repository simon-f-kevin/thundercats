using Game_Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class GravityComponent : Component
    {
        
        public float Gravity { get; set; }
        public bool inAir { get; set; } = false;

        public GravityComponent(Entity id) : base(id)
        {
            Gravity = 0.5f;
        }
    }
}
