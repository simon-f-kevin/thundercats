using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Game_Engine.Components
{
    public abstract class CollisionComponent : Component
    {
        private BoundingBox box;
        public BoundingBox BoundingBox
        {
            get {
                return box;
            }
            set {
                if (box != null)
                {
                    box = value;
                }
                else {
                    throw new Exception("Must be the same volume type as the initial volume");
                }
            }
        }

        private BoundingSphere sphere;
        public BoundingSphere BoundingSphere
        {
            get
            {
                return sphere;
            }
            set
            {
                if (sphere != null)
                {
                    sphere = value;
                }
                else
                {
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
    }
}
