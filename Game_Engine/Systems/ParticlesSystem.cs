//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Game_Engine.Managers;
//using Game_Engine.Components;
//using Game_Engine.Components.Particle;

//namespace Game_Engine.Systems
//{
//    public class ParticleSystem : IDrawableSystem
//    {
//        private GraphicsDeviceManager graphicsDeviceManager;
//        private GraphicsDevice graphicsDevice;

//        public ParticleSystem(GraphicsDeviceManager graphicsDeviceManager)
//        {
//            this.graphicsDeviceManager = graphicsDeviceManager;
//            graphicsDevice = graphicsDeviceManager.GraphicsDevice;
//        }

//        public void Draw(GameTime gameTime)
//        {
//            InitiateParticles();
//            ManipulateParticlePosition(gameTime);
//        }

//        /// <summary>
//        /// This method initates every ParticleComponent
//        /// </summary>
//        private void InitiateParticles()
//        {
//            var particleComponents = ComponentManager.Instance.GetConcurrentDictionary<ParticleComponent>();

//            foreach (var particleComponentKeyValuePair in particleComponents)
//            {
//                var particleComponent = particleComponentKeyValuePair.Value as ParticleComponent;
//                var transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(particleComponentKeyValuePair.Key);

//                var cameraComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CameraComponent>(particleComponentKeyValuePair.Key);

//                particleComponent.Vertices = new PositionTexcoordVertex[4 * particleComponent.NumOfParticles];

//                //var worldViewProjection = cameraComponent.WorldMatrix * cameraComponent.ViewMatrix * cameraComponent.ProjectionMatrix;
//                //var vertexPos1 = new Vector3(-0.26286500f, 0.0000000f, 0.42532500f);
//                //var vertexPos2 = new Vector3(-0.26286500f, 0.0000000f, -0.42532500f);
//                //var vertexPos3 = new Vector3(-0.26286500f, 0.0000000f, -0.42532500f);

//                #region Initializationbuffer 
//                //Sets up the vertex buffer
//                particleComponent.VertexBuffer = new VertexBuffer(graphicsDevice, typeof(PositionTexcoordVertex), 4 * particleComponent.NumOfParticles, BufferUsage.WriteOnly);


//                for (int i = 0; i < particleComponent.NumOfParticles; i++)
//                {

//                    particleComponent.Vertices[4 * i + 0] = new PositionTexcoordVertex(particleComponent.ParticlePosition, new Vector2(0, 0));
//                    particleComponent.Vertices[4 * i + 1] = new PositionTexcoordVertex(particleComponent.ParticlePosition, new Vector2(0, 1));
//                    particleComponent.Vertices[4 * i + 2] = new PositionTexcoordVertex(particleComponent.ParticlePosition, new Vector2(1, 1));
//                    particleComponent.Vertices[4 * i + 3] = new PositionTexcoordVertex(particleComponent.ParticlePosition, new Vector2(1, 0));
//                }
//                particleComponent.VertexBuffer.SetData(particleComponent.Vertices);

//                particleComponent.Indices = new ushort[6 * particleComponent.NumOfParticles];

//                for (int i = 0; i < particleComponent.NumOfParticles; i++)
//                {
//                    particleComponent.Indices[4 * i + 0] = (ushort)(4 * i + 0);
//                    particleComponent.Indices[4 * i + 1] = (ushort)(4 * i + 1);
//                    particleComponent.Indices[4 * i + 2] = (ushort)(4 * i + 2);
//                    particleComponent.Indices[4 * i + 3] = (ushort)(4 * i + 3);
//                    particleComponent.Indices[4 * i + 4] = (ushort)(4 * i + 4);
//                    particleComponent.Indices[4 * i + 5] = (ushort)(4 * i + 5);
//                }
//                particleComponent.IndexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), particleComponent.Indices.Length, BufferUsage.WriteOnly);
//                particleComponent.IndexBuffer.SetData(particleComponent.Indices);


//                graphicsDeviceManager.GraphicsDevice.SetVertexBuffer(particleComponent.VertexBuffer);
//                graphicsDeviceManager.GraphicsDevice.Indices = particleComponent.IndexBuffer;
//                #endregion Initializationbuffer   

//                // Particle.fx filen innehåller en viss information som måste matchas med vår PositionTexcoordVertex struct.
//                //Görs via Effect Objektet exempel nedan.
//                particleComponent.Effect = AssetManager.Instance.GetContent<Effect>("Particles");
//                particleComponent.Effect.CurrentTechnique = particleComponent.Effect.Techniques["ParticleDrawing"];
//                particleComponent.Effect.Parameters["WorldViewProjection"].SetValue(worldViewProjection);
//                graphicsDeviceManager.GraphicsDevice.Indices = particleComponent.IndexBuffer;
//                particleComponent.Effect.CurrentTechnique.Passes[0].Apply();

//                graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4 * particleComponent.NumOfParticles, 0, 2 * particleComponent.NumOfParticles);
                                          
//            }

//        }
        
//        public void ManipulateParticlePosition(GameTime gameTime)
//        {
//            var particleComponents = ComponentManager.Instance.GetConcurrentDictionary<ParticleComponent>();

//            foreach (var particleCOmponentKeyValuePair in particleComponents)
//            {
//                var particleComponent = particleCOmponentKeyValuePair.Value as ParticleComponent;
//                var cameraComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CameraComponent>(particleCOmponentKeyValuePair.Key);
//                var transform = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(particleCOmponentKeyValuePair.Key);
//                var velocity = ComponentManager.Instance.ConcurrentGetComponentOfEntity<VelocityComponent>(particleCOmponentKeyValuePair.Key);

//                var elapsedTime = gameTime.ElapsedGameTime.Milliseconds;

//                // Set a randomdirection for every particle
//                var randomDirection = new Vector3(0, 0, 0);
//                var randomValue = new Random();
//                randomDirection.X = RandomFloat(randomValue);
//                randomDirection.Y = RandomFloat(randomValue);
//                randomDirection.Z = RandomFloat(randomValue);

//                particleComponent.ParticlePosition = transform.Position;
                
//                // Particle Size
//                particleComponent.ParticleHeight = 0.1f;
//                particleComponent.ParticleWidth = 0.1f;

//                for (int i = 0; i < particleComponent.NumOfParticles; i++)
//                {
//                    // StartPosition
//                    particleComponent.ParticlePosition = transform.Position;
//                    // Hur länge partikeln har varit vid liv.
//                    particleComponent.Age = elapsedTime;
//                    particleComponent.Effect.Parameters["TotalTime"].SetValue(particleComponent.Age);
//                    // hur länge den ska vara vid liv
//                    particleComponent.LifeTime = 2f;
//                    particleComponent.Effect.Parameters["Life"].SetValue(particleComponent.LifeTime);

//                    while (particleComponent.Age < particleComponent.LifeTime)
//                    {
//                        particleComponent.ParticlePosition += randomDirection;
//                    }
//                }
//            }
//        }

//       public void PixelShader()
//        {
            
//        }
//        private float RandomFloat(Random random)
//        {
//            double val = random.NextDouble(); 
            
//            return  (float)val;
//        }
//    }
//}
