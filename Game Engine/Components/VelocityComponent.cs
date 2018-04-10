using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;
using Microsoft.Xna.Framework;

namespace Game_Engine.Components
{
    public class VelocityComponent : Component
    {
        public Vector3 Velocity;
        public Vector3 DefaultSpeed;

        public VelocityComponent(Entity id) : base(id)
        {
            
        }
        public VelocityComponent(Entity id, Vector3 velocity, Vector3 DefaultSpeed) : base(id)
        {
            this.Velocity = velocity;
            this.DefaultSpeed = DefaultSpeed;

        }
    }
}
