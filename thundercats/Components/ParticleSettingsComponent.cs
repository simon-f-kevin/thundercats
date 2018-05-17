using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thundercats.Components
{
    public class ParticleSettingsComponent : Component
    {
        public string TextureName { get; set; }
        public int MaximumParticles { get; set; }
        public TimeSpan ParticleLifeSpan { get; set; }
        public float EmitterVelocitySensitivity { get; set; }
        public float MinVelocity { get; set; }
        public float MaxVelocity { get; set; }
        public Vector3 GravityDirection { get; set; }
        public BlendState AlphaBlendState { get; set; }

        public ParticleSettingsComponent(Entity id) : base(id)
        {
        }
    }
}
