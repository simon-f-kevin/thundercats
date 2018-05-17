using Game_Engine.Components.Particle;
using Game_Engine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Components
{
    public class ParticleComponent : Component
    {
       // public VertexPositionColor[] Vertices { get; set; }
        public PositionTexcoordVertex[] vertices { get; set; }
        //public VertexPositionTexture[] verticess { get; set; }
        public Effect Effect { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 TexturePosition { get; set; }
        public ushort[] Indices { get; set; }
        public float ParticleHeight { get; set; }
        public float ParticleWidth { get; set; }
        public short NumOfParticles { get; set; } = 16;
        public Vector3 ParticlePosition { get; set; }
        public float LifeTime { get; set; }
        public float Age { get; set; }
        
        



        public ParticleComponent(Entity id, Effect effect) : base(id)
        {
            Effect = effect;
        }
    }
}
