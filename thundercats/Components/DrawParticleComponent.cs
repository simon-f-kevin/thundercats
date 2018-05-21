using System;
using System.Collections.Generic;
using Game_Engine.Components;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Entities;

namespace thundercats.Components
{
    /// <summary>
    /// If this component exist in an entity we will fire the particle
    /// creation system and draw particles on the players position.
    /// </summary>
    public class DrawParticleComponent : Component
    {
        public DrawParticleComponent(Entity id) : base(id)
        {
           
        }
    }
}
