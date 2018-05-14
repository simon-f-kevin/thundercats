//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Game_Engine.Components.Particle
//{
//    public class VertexType
//    {
//        public struct PositionTexcoordVertex : IVertexType
//        {
//            public Vector4 Position;
//            public Vector2 TextCoord;
//            public float Lifetime;
            

//            public VertexDeclaration VertexDeclaration
//            {
//                get
//                {
//                    return VertexDeclaration;
//                }
//            }
//            public PositionTexcoordVertex(Vector3 position, Vector2 textcoord, float Lifetime)
//            {
                
//                Position = new Vector4(position, 1.0f);
//                TextCoord = textcoord;
//                Lifetime = 2f;
                
//            }

//            private static VertexElement[] vertexElements = {
//                new VertexElement(0,VertexElementFormat.Vector4,VertexElementUsage.Position,0),
//                new VertexElement(4*sizeof(float), VertexElementFormat.Vector2,VertexElementUsage.TextureCoordinate,0)
//        };
//            private static VertexDeclaration vertexDeclaration = new VertexDeclaration(vertexElements);
//        }

//    }
//}
//}
