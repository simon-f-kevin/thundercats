using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Managers;
using Game_Engine.Components;
using Game_Engine.Components.Particle;

namespace Game_Engine.Systems
{
    public class ParticleSystem : IDrawableSystem
    {
        GraphicsDeviceManager modelManager;
        public void Draw(GameTime gameTime)
        {
            var billboardComponents = ComponentManager.Instance.GetConcurrentDictionary<BillboardComponent>();

            foreach(var billboardComponent in billboardComponents)
            {
                var billboard = billboardComponent.Value as BillboardComponent;

                //GenerateParticles(billboard.Particles)
                
            }
        }

        public void DrawParticle(GraphicsDevice device)
        {
            var billboardComponents = ComponentManager.Instance.GetConcurrentDictionary<BillboardComponent>();

            foreach (var billboardComponent in billboardComponents)
            {
                var billboard = billboardComponent.Value as BillboardComponent;

                billboard.StartPosition = new Vector3(0, 0, 0);
                billboard.Particles = new VertexPositionTexture[billboard.NumOfBillboards * 4];
                billboard.VertexGPUBuffer = new VertexBuffer(device, billboard.VertexDeclaration, 4 * billboard.NumOfBillboards, BufferUsage.WriteOnly);

                for (int i = 0; i < billboard.NumOfBillboards; i++)
                {
                    billboard.Particles[4*i + 0] = new VertexPositionTexture(billboard.StartPosition, new Vector2(0, 0));
                    billboard.Particles[4*i + 1] = new VertexPositionTexture(billboard.StartPosition, new Vector2(0, 1));
                    billboard.Particles[4*i + 2] = new VertexPositionTexture(billboard.StartPosition, new Vector2(1, 1));
                    billboard.Particles[4*i + 3] = new VertexPositionTexture(billboard.StartPosition, new Vector2(1, 0));

                    billboard.indices[6 * i + 0] = (ushort)(4 * i + 0);
                    billboard.indices[6 * i + 1] = (ushort)(4 * i + 1);
                    billboard.indices[6 * i + 2] = (ushort)(4 * i + 2);
                    billboard.indices[6 * i + 3] = (ushort)(4 * i + 3);
                    billboard.indices[6 * i + 4] = (ushort)(4 * i + 4);
                    billboard.indices[6 * i + 5] = (ushort)(4 * i + 5);

                    billboard.IndexBuffer = new IndexBuffer(device, typeof(ushort), billboard.indices.Length, BufferUsage.WriteOnly);
                    billboard.IndexBuffer.SetData(billboard.indices);

                    modelManager.GraphicsDevice.SetVertexBuffer(billboard.VertexGPUBuffer);
                    modelManager.GraphicsDevice.Indices = billboard.IndexBuffer;
                }
            }
        }
        
        public void GenerateParticles(Vector3[] particlePositions)
        {
            //var billboardComponents = ComponentManager.Instance.GetConcurrentDictionary<BillboardComponent>();

            //foreach (var billboardComponent in billboardComponents)
            //{
            //    var billboard = billboardComponent.Value as BillboardComponent;

            //    billboard.Particles = new VertexPositionTexture[billboard.NumOfBillboards * 4];
            //    int x = 0;

            //    for (int i = 0; i < billboard.NumOfBillboards*4; i+=4)
            //    {
            //        var pos = particlePositions[i / 4];

            //        billboard.Particles[i + 0] = new VertexPositionTexture(pos, new Vector2(0, 0));
            //        billboard.Particles[i + 1] = new VertexPositionTexture(pos, new Vector2(0, 1));
            //        billboard.Particles[i + 2] = new VertexPositionTexture(pos, new Vector2(1, 1));
            //        billboard.Particles[i + 3] = new VertexPositionTexture(pos, new Vector2(1, 0));

            //        //billboard.indices[x++] = i + 0;
            //        //billboard.indices[x++] = i + 3;
            //        //billboard.indices[x++] = i + 2;
            //        //billboard.indices[x++] = i + 2;
            //        //billboard.indices[x++] = i + 1;
            //        //billboard.indices[x++] = i + 0;

            //        billboard.VertexGPUBuffer = new VertexBuffer(device, typeof(VertexPositionTexture), billboard.NumOfBillboards * 4, BufferUsage.WriteOnly);
            //        //billboard.VertexGPUBuffer.SetData<int>(billboard.indices);
            //    }
            //}
        }
    }
}
