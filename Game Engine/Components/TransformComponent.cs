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
        public Vector3 position;
        public Vector3 scale;
        public Matrix rotationMatrix { get; set; }

        TransformComponent(Entity id) : base(id)
        {

        }
    }
}
