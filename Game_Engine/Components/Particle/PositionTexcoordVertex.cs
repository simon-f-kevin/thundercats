using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components.Particle
{
    public struct PositionTexcoordVertex : IVertexType
    {
        public Vector4 ParticlePosition;
        public Vector2 TexturePosition;

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return vertexDeclaration;
            }
        }
        public PositionTexcoordVertex(Vector3 position, Vector2 texturePosition)
        {

            ParticlePosition = new Vector4(position, 1.0f);
            TexturePosition = texturePosition;

        }

        private static VertexElement[] vertexElements = {
                new VertexElement(0,VertexElementFormat.Vector4,VertexElementUsage.Position,0),
                new VertexElement(4*sizeof(float), VertexElementFormat.Vector2,VertexElementUsage.TextureCoordinate,0)
        };

        private static VertexDeclaration vertexDeclaration = new VertexDeclaration(vertexElements);
    }

}


