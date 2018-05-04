using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Game_Engine.Components
{
    public class BoundingSphereComponent : CollisionComponent
    {
        public BoundingSphereComponent(Entity id, BoundingSphere sphere) : base(id)
        {
            this.BoundingShape = sphere;
        }

        public override Vector3 Center => ((BoundingSphere)base.BoundingShape).Center;
    }
}
