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
    public class BillboardComponent : Component
    {
        public Vector3 StartPosition;
        public VertexPositionTexture[] Particles { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public VertexBuffer VertexGPUBuffer { get; set; }
        public VertexType VertexCPUBuffer { get; set; }
        public VertexDeclaration VertexDeclaration { get; set; }
        Texture2D Texture { get; set; }
        Vector2 BillboardSize { get; set; }
        public Effect Effect { get; set; }
        public int NumOfBillboards { get; set; }
        public GraphicsDevice Device { get; set; }
        public ushort[] indices { get; set; }

        public BillboardComponent(Entity id, GraphicsDevice device, ContentManager content, Texture2D texture, Vector2 billboardSize, Vector3[] particlePosition) : base(id)
        {
            NumOfBillboards = particlePosition.Length;
            BillboardSize = billboardSize;
            Device = device;
            Texture = texture;
            Effect = content.Load<Effect>("BillboardEffect");
        }
    }
}
