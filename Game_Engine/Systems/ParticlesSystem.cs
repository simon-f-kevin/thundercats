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
            DrawParticles();
            ManipulateParticlePosition(gameTime);
        }

        /// <summary>
        /// This method initates every ParticleComponent
        /// </summary>
        private void DrawParticles()
        {
            var particleComponents = ComponentManager.Instance.GetConcurrentDictionary<ParticleComponent>();

            foreach (var particleComponentKeyValuePair in particleComponents)
            {
                var particleComponent = particleComponentKeyValuePair.Value as ParticleComponent;
                var transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(particleComponentKeyValuePair.Key);

                particleComponent.Vertices = new PositionTexcoordVertex[4*particleComponent.NumOfParticles];
    
                var vertexPos1 = new Vector3(-0.26286500f, 0.0000000f, 0.42532500f);
                var vertexPos2 = new Vector3(-0.26286500f, 0.0000000f, -0.42532500f);
                var vertexPos3 = new Vector3(-0.26286500f, 0.0000000f, -0.42532500f);

                //Sets up the vertex buffer
                particleComponent.VertexBuffer = new VertexBuffer(device, typeof(PositionTexcoordVertex), 4*particleComponent.NumOfParticles, BufferUsage.WriteOnly);

                
                for (int i = 0; i < particleComponent.NumOfParticles; i++)
                {
                    
                    particleComponent.Vertices[4 * i + 0] = new PositionTexcoordVertex(particleComponent.ParticlePosition, new Vector2(0,0));
                    particleComponent.Vertices[4 * i + 1] = new PositionTexcoordVertex(particleComponent.ParticlePosition, new Vector2(0,1));
                    particleComponent.Vertices[4 * i + 2] = new PositionTexcoordVertex(particleComponent.ParticlePosition, new Vector2(1,1));
                    particleComponent.Vertices[4 * i + 3] = new PositionTexcoordVertex(particleComponent.ParticlePosition, new Vector2(1,0));
                }
                particleComponent.VertexBuffer.SetData(particleComponent.Vertices);

                particleComponent.Indices = new ushort[6 * particleComponent.NumOfParticles];

                for (int i = 0; i < particleComponent.NumOfParticles; i++)
                {
                    particleComponent.Indices[6 * i + 0] = (ushort)(4 * i + 0);
                    particleComponent.Indices[6 * i + 1] = (ushort)(4 * i + 1);
                    particleComponent.Indices[6 * i + 2] = (ushort)(4 * i + 2);
                    particleComponent.Indices[6 * i + 3] = (ushort)(4 * i + 3);
                    particleComponent.Indices[6 * i + 4] = (ushort)(4 * i + 4);
                    particleComponent.Indices[6 * i + 5] = (ushort)(4 * i + 5);
                }
                particleComponent.IndexBuffer = new IndexBuffer(device, typeof(ushort), particleComponent.Indices.Length, BufferUsage.WriteOnly);
                particleComponent.IndexBuffer.SetData(particleComponent.Indices);

                modelManager.GraphicsDevice.SetVertexBuffer(particleComponent.VertexBuffer);
                modelManager.GraphicsDevice.Indices = particleComponent.IndexBuffer;

                // Particle.fx filen innehåller en viss information som måste matchas med vår PositionTexcoordVertex struct.
                //Görs via Effect Objektet exempel nedan.
                
                //particle.Effect.Parameters[" WorldViewProjection"].SetValue();
               
                //particle.Effect seems to always be null
                particleComponent.Effect.CurrentTechnique = particleComponent.Effect.Techniques["TransformAndTexture"];

                modelManager.GraphicsDevice.Indices = particleComponent.IndexBuffer;
                // modelManager.GraphicsDevice.VertexDeclaration = particle.Vertices[].VertexDeclaration;

                foreach (EffectPass pass in particleComponent.Effect.CurrentTechnique.Passes)
                {

                    //modelManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4 * particle.NumOfParticles, 0, 2 * particle.NumOfParticles);
                    modelManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, particleComponent.NumOfParticles);
                    

                    pass.Apply();
                }           
            }

        }
        
        public void ManipulateParticlePosition(GameTime gameTime)
        {
            var particleComponents = ComponentManager.Instance.GetConcurrentDictionary<ParticleComponent>();

            foreach (var particleComponent in particleComponents)
            {
                var particle = particleComponent.Value as ParticleComponent;
                var camera = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CameraComponent>(particleComponent.Key);
                var transform = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(particleComponent.Key);
                var velocity = ComponentManager.Instance.ConcurrentGetComponentOfEntity<VelocityComponent>(particleComponent.Key);

                var elapsedTime = gameTime.ElapsedGameTime.Milliseconds;

                // Set a randomdirection for every particle
                var randomDirection = new Vector3(0, 0, 0);
                var randomValue = new Random();
                randomDirection.X = RandomFloat(randomValue);
                randomDirection.Y = RandomFloat(randomValue);
                randomDirection.Z = RandomFloat(randomValue);

                particle.ParticlePosition = transform.Position;
                
                // Particle Size
                particle.ParticleHeight = 0.1f;
                particle.ParticleWidth = 0.1f;

                for (int i = 0; i < particle.NumOfParticles; i++)
                {
                    // StartPosition
                    particle.ParticlePosition = transform.Position;
                    // Hur länge partikeln har varit vid liv.
                    particle.Age = elapsedTime;
                    // hur länge den ska vara vid liv
                    particle.LifeTime = 2f;

                    while (particle.Age < particle.LifeTime)
                    {
                        particle.ParticlePosition += randomDirection;
                    }
                }
            }
        }

       public void PixelShader()
        {
            
        }
        private float RandomFloat(Random random)
        {
            double val = random.NextDouble(); 
            
            return  (float)val;
        }
    }
}
