using Game_Engine.Components.Particle;
using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components
{
    public class ParticleComponent : Component
    {
        public VertexPositionColor[] Vertices { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public ushort[] Indices { get; set; }
        public float ParticleHeight { get; set; }
        public float ParticleWidth { get; set; }
        public short NumOfParticles { get; set; }
        



        public ParticleComponent(Entity id) : base(id)
        {
           
        }
    }
}
