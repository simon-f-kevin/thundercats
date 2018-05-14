using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class BoundingBoxComponent : CollisionComponent
    {
        public BoundingBoxComponent(Entity id, BoundingBox box) : base(id)
        {
            this.BoundingBox = box;
        }

        public override List<BoundingBox> Children{ get; set; }

        public override Vector3 Center
        {
            get => ((BoundingBox)base.BoundingBox).Min + ((((BoundingBox)base.BoundingBox).Max - ((BoundingBox)base.BoundingBox).Min) / 2);
        }
    }
}
