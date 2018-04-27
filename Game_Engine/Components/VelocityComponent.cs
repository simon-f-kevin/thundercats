using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;

namespace Game_Engine.Components
{
    public class VelocityComponent : Component
    {
        public State<Vector3> Velocity { get; set; }
        public Vector3 DefaultSpeed;

        public VelocityComponent(Entity id) : base(id)
        {
            Velocity = new State<Vector3>(Vector3.Zero);
        }
        public VelocityComponent(Entity id, Vector3 velocity, Vector3 DefaultSpeed) : base(id)
        {
            this.Velocity.New = Velocity.Old + velocity;
            this.DefaultSpeed = DefaultSpeed;

        }
    }
}
