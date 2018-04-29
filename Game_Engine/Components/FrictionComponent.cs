using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;

namespace Game_Engine.Components
{
 
    public class FrictionComponent : Component
    {
        public float Friction { get; set; } = 0;
        public FrictionComponent(Entity id) : base(id)
        {
        }
    }
}
