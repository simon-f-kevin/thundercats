using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Game_Engine.Components
{
    public class CollisionComponent : Component
    {
        public BoundingVolume BoundingVolume { get; set; }

        public CollisionComponent(Entity id, BoundingVolume volume) : base(id)
        {
            BoundingVolume = volume;
        }
    }
}
