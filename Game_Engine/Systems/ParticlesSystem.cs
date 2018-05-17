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

        public void DrawParticles()
        {
            var particleComponents = ComponentManager.Instance.GetConcurrentDictionary<ParticleComponent>();

            foreach (var particleComponent in particleComponents)
            {
                var particle = particleComponent.Value as ParticleComponent;
                var transform = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(particleComponent.Key);
                var camera = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CameraComponent>(particleComponent.Key);

                particle.vertices = new PositionTexcoordVertex[4*particle.NumOfParticles];

                var worldviewproj = camera.WorldMatrix * camera.ViewMatrix * camera.ProjectionMatrix;
                //var vertexPos1 = new Vector3(-0.26286500f, 0.0000000f, 0.42532500f);
                //var vertexPos2 = new Vector3(-0.26286500f, 0.0000000f, -0.42532500f);
                //var vertexPos3 = new Vector3(-0.26286500f, 0.0000000f, -0.42532500f);

                #region Initializationbuffer 
                //Sets up the vertex buffer
                particle.VertexBuffer = new VertexBuffer(device, typeof(PositionTexcoordVertex), 4*particle.NumOfParticles, BufferUsage.WriteOnly);

                for (int i = 0; i < particle.NumOfParticles; i++)
                {
                    
                    particle.vertices[4 * i + 0] = new PositionTexcoordVertex(particle.ParticlePosition, new Vector2(0,0));
                    particle.vertices[4 * i + 1] = new PositionTexcoordVertex(particle.ParticlePosition, new Vector2(0,1));
                    particle.vertices[4 * i + 2] = new PositionTexcoordVertex(particle.ParticlePosition, new Vector2(1,1));
                    particle.vertices[4 * i + 3] = new PositionTexcoordVertex(particle.ParticlePosition, new Vector2(1,0));
                }
                particle.VertexBuffer.SetData(particle.vertices);

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
                #endregion Initializationbuffer   

                // Particle.fx filen innehåller en viss information som måste matchas med vår PositionTexcoordVertex struct.
                //Görs via Effect Objektet exempel nedan.
                particle.Effect.CurrentTechnique = particle.Effect.Techniques["TransformAndTexture"];
                particle.Effect.Parameters["WorldViewProjection"].SetValue(worldviewproj);
                modelManager.GraphicsDevice.Indices = particle.IndexBuffer;
                particle.Effect.CurrentTechnique.Passes[0].Apply();

                modelManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4 * particle.NumOfParticles, 0, 2 * particle.NumOfParticles);
                                          
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
