using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;

namespace Game_Engine.Components
{
    public class VelocityComponent : Component
    {
        public Vector3 Velocity;

        public VelocityComponent(Entity id) : base(id)
        {
            Velocity = Vector3.Zero;
        }
        public VelocityComponent(Entity id, Vector3 velocity) : base(id)
        {
            Velocity = velocity;
        }
    }
}
