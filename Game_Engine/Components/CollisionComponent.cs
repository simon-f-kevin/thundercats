using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public abstract class CollisionComponent : Component
    {
        public dynamic BoundingShape { get; protected set; }

        public abstract Vector3 Center { get; }

        public CollisionComponent(Entity id) : base(id)
        {
        }

        /// <summary>
        /// Makes it easy to check intersections with different boundingvolumes that have 
        /// the underlying intersects method.
        /// </summary>
        /// <param name="collisionComponent"></param>
        /// <returns></returns>
        public bool Intersects(CollisionComponent collisionComponent)
        {
            return BoundingShape.Intersects(collisionComponent.BoundingShape);
        }

        public void SetVolume(dynamic shape)
        {
            // Must be the same type as the initial shape so we don't get any errors
            if (shape.GetType() == this.BoundingShape.GetType())
                this.BoundingShape = shape;
            else throw new Exception("Must be the same volume type as the initial volume");
        }
    }
}
