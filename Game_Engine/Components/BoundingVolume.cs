


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;
using Microsoft.Xna.Framework;

namespace Game_Engine.Components
{
    public abstract class BoundingVolume
    {
        public enum VolumeType
        {
            Sphere,
            Box,
        }

        // Properties

        public BoundingBox BoundingBox { get; set; }
        public BoundingSphere BoundingSphere { get; set; }
            
        public VolumeType Type { get; protected set; }

        public abstract Vector3 Center { get; }
        public bool LandedOnBlock { get; set; }

        // Methods

        public abstract void UpdateVolume(Vector3 position);
        public abstract bool Intersects(BoundingVolume volume);


        //public abstract List<BoundingBox> Children { get; set; }

        public BoundingBox GetVolume()
        {
            return BoundingBox;
        }

    }
}
