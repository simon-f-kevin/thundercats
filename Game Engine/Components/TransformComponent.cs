using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class TransformComponent : Component
    {
        public Vector3 position { get; set; }
        public Vector3 scale { get; set; }
        public Matrix rotationMatrix { get; set; }

        public TransformComponent(Entity id) : base(id)
        {
            Position = new Vector3(0, 0, 0);
        }

        public TransformComponent(Entity id, Vector3 pos) : base(id)
        {
            Position = pos;
        }
    }
}
