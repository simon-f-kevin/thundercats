using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Game_Engine.Components
{
    public abstract class CollisionComponent : Component
    {
        private dynamic boundingShape;
        public dynamic BoundingShape
        {
            get {
                return boundingShape;
            }
            set {
                if (boundingShape == null || value.GetType() == boundingShape.GetType())
                {
                    boundingShape = value;
                }
                else {
                    throw new Exception("Must be the same volume type as the initial volume");
                }
            }
        }
        public abstract List<BoundingBox> Children { get; set; }

        public abstract Vector3 Center { get; }

        public CollisionComponent(Entity id) : base(id)
        {
            Children = new List<BoundingBox>();
        }

        public abstract void UpdateShape(Vector3 position);

        public bool LandedOnBlock { get; set; }
    }
}
