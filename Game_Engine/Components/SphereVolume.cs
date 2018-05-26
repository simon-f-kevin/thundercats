using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Game_Engine.Components
{
    public class SphereVolume : BoundingVolume
    {
        public SphereVolume(BoundingSphere sphere)
        {
            Type = VolumeType.Sphere;
            BoundingSphere = sphere;
        }

        public override Vector3 Center => BoundingSphere.Center;

        public override void UpdateVolume(Vector3 position)
        {
            BoundingSphere = new BoundingSphere(position, BoundingSphere.Radius);
        }


        public override bool Intersects(BoundingVolume volume)
        {
            switch (volume.Type)
            {
                case VolumeType.Box:
                    return BoundingSphere.Intersects(volume.BoundingBox);
                case VolumeType.Sphere:
                    return BoundingSphere.Intersects(volume.BoundingSphere);
                default:
                    return false;
            }           
        }
    }
}
