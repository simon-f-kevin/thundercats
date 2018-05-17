using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using thundercats.GameStates;

namespace thundercats
{
    /*
     * Entity factory for game specific objects
     */
    public static class GameEntityFactory
    {
        public static GraphicsDevice GraphicsDevice;
        public static Entity NewPlayer(String model, int gamePadIndex, Vector3 transformPos, Texture2D texture)
        {
            Entity player = EntityFactory.NewEntity("local_player");
            TransformComponent transformComponent = new TransformComponent(player, transformPos);
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>(model));
            VelocityComponent velocityComponent = new VelocityComponent(player);
            CollisionComponent collisionComponent = new BoundingSphereComponent(player, modelComponent.Model.Meshes[0].BoundingSphere);
            PlayerComponent playerComponent = new PlayerComponent(player);
            KeyboardComponent keyboardComponent = new KeyboardComponent(player);
            GamePadComponent gamePadComponent = new GamePadComponent(player, gamePadIndex);
            FrictionComponent frictionComponent = new FrictionComponent(player);
            TextureComponent textureComponent = new TextureComponent(player, texture);
            ParticleComponent particleComponent = new ParticleComponent(player)
            {
                LifeTime = 2,
                Age = 0,
                NumOfParticles = 2,
                Texture = AssetManager.Instance.CreateTexture(Color.Aqua, GraphicsDevice),
                TexturePosition = new Vector2(0, 0),
            };
            
            

            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, collisionComponent, typeof(CollisionComponent));
            ComponentManager.Instance.AddComponentToEntity(player, playerComponent);
            ComponentManager.Instance.AddComponentToEntity(player, keyboardComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gamePadComponent);
            ComponentManager.Instance.AddComponentToEntity(player, frictionComponent);
            ComponentManager.Instance.AddComponentToEntity(player, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(player, particleComponent);

            PhysicsSystem.SetInitialModelPos(modelComponent, transformComponent);
            PhysicsSystem.SetInitialBoundingSpherePos(collisionComponent, transformComponent);

            return player;
        }

        public static Entity NewPlayerWithCamera(String model, int gamePadIndex, Vector3 transformPos, Vector3 cameraPos, float cameraAspectRatio, bool followPlayer, Texture2D texture)
        {
            Entity player = NewPlayer(model, gamePadIndex, transformPos, texture);
            CameraComponent cameraComponent = new CameraComponent(player, cameraPos, cameraAspectRatio, followPlayer);

            ComponentManager.Instance.AddComponentToEntity(player, cameraComponent);

            return player;
        }

        public static Entity NewBlock(Vector3 positionValues, Texture2D texture)
        {
            Entity block = EntityFactory.NewEntity();
            TransformComponent transformComponent = new TransformComponent(block, new Vector3(x: positionValues.X, y: positionValues.Y, z: positionValues.Z));
            ModelComponent modelComponent = new ModelComponent(block, AssetManager.Instance.GetContent<Model>("Models/Block"));
            modelComponent.World = Matrix.CreateWorld(transformComponent.Position, Vector3.Forward, Vector3.Up);
            TextureComponent textureComponent = new TextureComponent(block, texture);
            CollisionComponent collisionComponent = new BoundingBoxComponent(block, CreateBoundingBox(modelComponent.Model));
            
            BlockComponent blockComponent = new BlockComponent(block);

            ComponentManager.Instance.AddComponentToEntity(block, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(block, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(block, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(block, collisionComponent, typeof(CollisionComponent));
            ComponentManager.Instance.AddComponentToEntity(block, blockComponent);

            PhysicsSystem.SetInitialModelPos(modelComponent, transformComponent);
            PhysicsSystem.SetInitialBoundingBox(collisionComponent, transformComponent);

            return block;
        }

        public static void TestCollisionEntity(String model, Vector3 transformPos)
        {
            //Below is a temporary object you can use to test collision
            Entity blob = EntityFactory.NewEntity();
            ModelComponent modelComponent = new ModelComponent(blob, AssetManager.Instance.GetContent<Model>(model));
            TransformComponent transformComponent = new TransformComponent(blob, transformPos);
            CollisionComponent boundingSphereComponent = new BoundingSphereComponent(blob, modelComponent.Model.Meshes[0].BoundingSphere);

            ComponentManager.Instance.AddComponentToEntity(blob, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, boundingSphereComponent, typeof(CollisionComponent));

            PhysicsSystem.SetInitialModelPos(modelComponent, transformComponent);
            PhysicsSystem.SetInitialBoundingSpherePos(boundingSphereComponent, transformComponent);
        }

      
        private static BoundingBox CreateBoundingBox(Model model)
        {
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            BoundingBox result = new BoundingBox();
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    BoundingBox? meshPartBoundingBox = GetBoundingBox(meshPart, boneTransforms[mesh.ParentBone.Index]);
                    if (meshPartBoundingBox != null)
                        result = BoundingBox.CreateMerged(result, meshPartBoundingBox.Value);
                }
            return result;
        }

        private static BoundingBox? GetBoundingBox(ModelMeshPart meshPart, Matrix transform)
        {
            if (meshPart.VertexBuffer == null)
                return null;

            Vector3[] positions = VertexElementExtractor.GetVertexElement(meshPart, VertexElementUsage.Position);
            if (positions == null)
                return null;

            Vector3[] transformedPositions = new Vector3[positions.Length];
            Vector3.Transform(positions, ref transform, transformedPositions);

            return BoundingBox.CreateFromPoints(transformedPositions);
        }
        public static class VertexElementExtractor
        {
            public static Vector3[] GetVertexElement(ModelMeshPart meshPart, VertexElementUsage usage)
            {
                VertexDeclaration vd = meshPart.VertexBuffer.VertexDeclaration;
                VertexElement[] elements = vd.GetVertexElements();

                Func<VertexElement, bool> elementPredicate = ve => ve.VertexElementUsage == usage && ve.VertexElementFormat == VertexElementFormat.Vector3;
                if (!elements.Any(elementPredicate))
                    return null;

                VertexElement element = elements.First(elementPredicate);

                Vector3[] vertexData = new Vector3[meshPart.NumVertices];
                meshPart.VertexBuffer.GetData((meshPart.VertexOffset * vd.VertexStride) + element.Offset,
                    vertexData, 0, vertexData.Length, vd.VertexStride);

                return vertexData;
            }
        }
    }
}
