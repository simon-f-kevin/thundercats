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
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Matrix RotationMatrix { get; set; }

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
