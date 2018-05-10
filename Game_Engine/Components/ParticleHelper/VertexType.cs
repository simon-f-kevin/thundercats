using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Components.ParticleHelper
{
    public class VertexType
    {
        public struct PositionTextcoordVertex : IVertexType
        {
            public Vector4 Position;
            public Vector2 TextCoord;

            public VertexDeclaration VertexDeclaration
            {
                get
                {
                    return VertexDeclaration;
                }
            }
            public PositionTextcoordVertex(Vector3 position, Vector2 textcoord)
            {
                Position = new Vector4(position, 1.0f);
                TextCoord = textcoord;
            }

            private static VertexElement[] vertexElements = {
                new VertexElement(0,VertexElementFormat.Vector4,VertexElementUsage.Position,0),
                new VertexElement(4*sizeof(float), VertexElementFormat.Vector2,VertexElementUsage.TextureCoordinate,0)
        };
            private static VertexDeclaration vertexDeclaration = new VertexDeclaration(vertexElements);
        }

    }
}
}
