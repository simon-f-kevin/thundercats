using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class BoxVolume : BoundingVolume
    {

        public override Vector3 Center => (BoundingBox).Min + (((BoundingBox).Max - (BoundingBox).Min) / 2);

        public BoxVolume(BoundingBox box)
        {
            Type = VolumeType.Box;
            BoundingBox = box;
        }

        public override void UpdateVolume(Vector3 position)
        {
            Vector3 size = BoundingBox.Max - BoundingBox.Min;
            BoundingBox = new BoundingBox(position - size / 2, position + size / 2);
        }

        public override bool Intersects(BoundingVolume volume)
        {
            switch (volume.Type)
            {
                case VolumeType.Box:
                    return BoundingBox.Intersects(volume.BoundingBox);
                case VolumeType.Sphere:
                    return BoundingBox.Intersects(volume.BoundingSphere);
                default:
                    return false;
            }
        }
    }


}
