using Game_Engine.Components;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Game_Engine.Helpers;
using thundercats.GameStates;

namespace thundercats.Systems
{
    #region structs
    public struct VertexData : IVertexType
    {
        public static readonly VertexDeclaration VertexDeclaration;
        public Vector3 Position;
        public Vector2 TextureCoodinates;

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        static VertexData()
        {
            var elems = new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
            };
            VertexDeclaration = new VertexDeclaration(elems);
        }
    }

    public struct InstanceData : IVertexType
    {
        public static readonly VertexDeclaration VertexDeclaration;

        public Vector3 RandomIntervals;
        public float ElapsedTime;

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        static InstanceData()
        {
            var elems = new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Single, VertexElementUsage.BlendWeight, 0)
            };
            VertexDeclaration = new VertexDeclaration(elems);
        }
    }
    #endregion  
    public class ParticleDrawSystem : IDrawableSystem
    {
        //instance variables
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private VertexBufferBinding bufferBinding;

        private int numberOfInstancesToDraw;
        private InstanceData[] instanceData;
        private VertexBuffer instanceBuffer;
        private VertexBufferBinding instanceBufferBinding;

        private Vector2 radiusRange;

        private float elapsedTime = 0;

        private GraphicsDevice graphicsDevice;

        private Random rnd;

        //current component values
        private Vector2 ParticleSize;
        private Texture2D ParticleTexture;
        private float Life;
        private uint LightEmitRate;
        private float ParticleRadius;
        private Vector2 RadiusDeviation;

        public Effect ParticleEffect { get; set; }

        public int MaxVisibleParticles { get; private set; }

        public ParticleDrawSystem(GraphicsDevice device)
        {
            graphicsDevice = device;
        }

        public void RefreshBuffers()
        {
            float halfWidth = ParticleSize.X / 2;
            float halfHeight = ParticleSize.Y / 2;

            VertexData[] vertices = new VertexData[4];
            vertices[0].Position = new Vector3(-halfWidth, -halfHeight, 0);
            vertices[1].Position = new Vector3(halfWidth, -halfHeight, 0);
            vertices[2].Position = new Vector3(-halfWidth, halfHeight, 0);
            vertices[3].Position = new Vector3(halfWidth, halfHeight, 0);

            vertices[0].TextureCoodinates = new Vector2(0.0f, 0.0f);
            vertices[1].TextureCoodinates = new Vector2(1.0f, 0.0f);
            vertices[2].TextureCoodinates = new Vector2(0.0f, 1.0f);
            vertices[3].TextureCoodinates = new Vector2(1.0f, 1.0f);

            vertexBuffer = new VertexBuffer(graphicsDevice, VertexData.VertexDeclaration, 4, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            bufferBinding = new VertexBufferBinding(vertexBuffer);

            int[] indices = new int[6];
            indices[0] = 1;
            indices[1] = 0;
            indices[2] = 2;
            indices[3] = 3;
            indices[4] = 1;
            indices[5] = 2;

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(int), 6, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);

            MaxVisibleParticles = (int)Math.Max(Math.Ceiling(Life * LightEmitRate), 1);

            instanceData = new InstanceData[MaxVisibleParticles];
            instanceBuffer = new VertexBuffer(graphicsDevice, InstanceData.VertexDeclaration, MaxVisibleParticles, BufferUsage.WriteOnly);
            instanceBufferBinding = new VertexBufferBinding(instanceBuffer, 0, 1);

            rnd = new Random();

            // Initialise our instance buffer
            for (int i = 0; i < MaxVisibleParticles; ++i)
            {
                instanceData[i].ElapsedTime = -(i + 1) / (float)LightEmitRate;
                instanceData[i].RandomIntervals = new Vector3(
                    rnd.Next(0, MaxVisibleParticles + 1) / (float)MaxVisibleParticles,
                    rnd.Next(0, MaxVisibleParticles + 1) / (float)MaxVisibleParticles,
                    rnd.Next(0, MaxVisibleParticles + 1) / (float)MaxVisibleParticles);
            }

            instanceBuffer.SetData(instanceData);

            radiusRange = new Vector2(ParticleRadius + RadiusDeviation.X, ParticleRadius + RadiusDeviation.Y);
        }

        public void Draw(GameTime gameTime)
        {
            UpdateParticleTime(gameTime.ElapsedGameTime.Seconds);
            var ParticleComponents = ComponentManager.Instance.GetConcurrentDictionary<ParticleComponent>();
            foreach (var particleComponentKeyValuePair in ParticleComponents)
            {
                var cameraComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CameraComponent>(particleComponentKeyValuePair.Key);
                var transformComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(particleComponentKeyValuePair.Key);
                var modelComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<ModelComponent>(particleComponentKeyValuePair.Key);
                var particleComponent = particleComponentKeyValuePair.Value as ParticleComponent;
                SetParticleData(particleComponent);
                RefreshBuffers();
                // Create our camera that's looking straight at the center
                Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 5.0f), Vector3.Zero, Vector3.Up);

                // Create a projection matrix so that the world dimensions have the same aspect ratio as our window size
                Matrix projMatrix = Matrix.CreateOrthographic(cameraComponent.AspectRatio * 300, 300, 0.1f, 10000);
                var worldViewProj = (viewMatrix * projMatrix);
                Console.WriteLine("particle: " + worldViewProj.Translation.ToString());
                DrawParticles(ref worldViewProj);
            }
           
        }

        private void UpdateParticleTime(float seconds)
        {
            numberOfInstancesToDraw = MaxVisibleParticles;

            elapsedTime += seconds;
        }


        private void SetParticleData(ParticleComponent component)
        {
            Life = component.LifeTime;
            ParticleSize = component.ParticleSize;
            ParticleTexture = component.Texture;
            LightEmitRate = component.EmitRate;
            ParticleRadius = component.Radius;
            RadiusDeviation = component.RadiusDeviation;
        }



        private void DrawParticles(ref Matrix worldViewProjection)
        {
            graphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.Black, 1.0f, 0);
            graphicsDevice.BlendState = BlendState.Additive;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            ParticleEffect.CurrentTechnique = ParticleEffect.Techniques["ParticleDrawing"];
            ParticleEffect.Parameters["WorldViewProjection"].SetValue(worldViewProjection);
            ParticleEffect.Parameters["Life"].SetValue(Life);
            ParticleEffect.Parameters["RadiusRange"].SetValue(radiusRange);
            ParticleEffect.Parameters["MaxNumOfParticles"].SetValue(MaxVisibleParticles);
            ParticleEffect.Parameters["TotalTime"].SetValue((float)elapsedTime);
            ParticleEffect.CurrentTechnique.Passes[0].Apply();

            graphicsDevice.Textures[0] = ParticleTexture;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            graphicsDevice.SetVertexBuffers(bufferBinding, instanceBufferBinding);
            graphicsDevice.Indices = indexBuffer;


            graphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 6, 0, 2, numberOfInstancesToDraw);

        }
    }
}
