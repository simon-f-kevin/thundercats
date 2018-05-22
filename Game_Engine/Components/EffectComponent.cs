using Game_Engine.Entities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class EffectComponent : Component
    {
        public Effect Effect { get; set; }

        public EffectComponent(Entity id, Effect effect) : base(id)
        {
            Effect = effect;
        }
    }
}
