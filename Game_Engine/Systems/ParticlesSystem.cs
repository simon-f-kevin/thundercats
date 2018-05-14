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
        public GraphicsDeviceManager modelManager { get; set; }
        public GraphicsDevice device { get; set; }
        public void Draw(GameTime gameTime)
        {
            var billboardComponents = ComponentManager.Instance.GetConcurrentDictionary<ParticleComponent>();

            foreach (var billboardComponent in billboardComponents)
            {
                var particle = billboardComponent.Value as ParticleComponent;



            }
        }

        public void DrawParticles()
        {
            var particleComponents = ComponentManager.Instance.GetConcurrentDictionary<ParticleComponent>();

            foreach (var particleComponent in particleComponents)
            {
                var particle = particleComponent.Value as ParticleComponent;
                var transform = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(particleComponent.Key);

                particle.Vertices = new VertexPositionColor[6];

                
                var vertexPos1 = new Vector3(-0.26286500f, 0.0000000f, 0.42532500f);
                var vertexPos2 = new Vector3(-0.26286500f, 0.0000000f, -0.42532500f);
                var vertexPos3 = new Vector3(-0.26286500f, 0.0000000f, -0.42532500f);

                //Sets up the vertex buffer
                particle.VertexBuffer = new VertexBuffer(device, typeof(VertexPositionColor), 4*particle.NumOfParticles, BufferUsage.WriteOnly);
                

                for (int i = 0; i < particle.NumOfParticles; i++)
                {
                    // Set Particle Color 
                    particle.Vertices[4*i+0] = new VertexPositionColor(vertexPos1, Color.Yellow);
                    particle.Vertices[4*i+1] = new VertexPositionColor(vertexPos2, Color.Orange);
                    particle.Vertices[4*i+2] = new VertexPositionColor(vertexPos3, Color.Red);
                }
                particle.VertexBuffer.SetData(particle.Vertices);

                particle.Indices = new ushort[6 * particle.NumOfParticles];

                for (int i = 0; i < particle.NumOfParticles; i++)
                {
                    particle.Indices[6 * i + 0] = (ushort)(4 * i + 0);
                    particle.Indices[6 * i + 1] = (ushort)(4 * i + 1);
                    particle.Indices[6 * i + 2] = (ushort)(4 * i + 2);
                    particle.Indices[6 * i + 3] = (ushort)(4 * i + 3);
                    particle.Indices[6 * i + 4] = (ushort)(4 * i + 4);
                    particle.Indices[6 * i + 5] = (ushort)(4 * i + 5);
                }
                particle.IndexBuffer = new IndexBuffer(device, typeof(ushort), particle.Indices.Length, BufferUsage.WriteOnly);
                particle.IndexBuffer.SetData(particle.Indices);

                modelManager.GraphicsDevice.SetVertexBuffer(particle.VertexBuffer);
                modelManager.GraphicsDevice.Indices = particle.IndexBuffer;
                modelManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4 * particle.NumOfParticles, 0, 2 * particle.NumOfParticles);
            }
        }

    }
}