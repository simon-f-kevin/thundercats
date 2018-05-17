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
            this.BoundingShape = box;
        }

        public override List<BoundingBox> Children{ get; set; }

        public override Vector3 Center
        {
            get => ((BoundingBox)base.BoundingShape).Min + ((((BoundingBox)base.BoundingShape).Max - ((BoundingBox)base.BoundingShape).Min) / 2);
        }

        public override void UpdateShape(Vector3 position)
        {
            Vector3 size = BoundingShape.Max - BoundingShape.Min;
            BoundingShape = new BoundingBox(position - size / 2, position + size / 2);
        }
    }
}
