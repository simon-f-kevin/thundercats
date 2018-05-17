using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Helpers;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using thundercats.Components;
using thundercats.GameStates;

namespace thundercats
{
    /*
     * Entity factory for game specific objects
     */
    public static class GameEntityFactory
    {
        public const string REMOTE_PLAYER = "remote_player";
        public const string LOCAL_PLAYER = "local_player";
        public const string AI_PLAYER = "ai_player";
        public const string BLOCK = "block";
        public const string GOAL = "Goal";

        public static Entity NewBasePlayer(String model, int gamePadIndex, Vector3 transformPos, Texture2D texture, String typeName)
        {
            Entity player = EntityFactory.NewEntity(typeName);
            TransformComponent transformComponent = new TransformComponent(player, transformPos);
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>(model));
            VelocityComponent velocityComponent = new VelocityComponent(player);
            CollisionComponent collisionComponent = new BoundingSphereComponent(player, modelComponent.Model.Meshes[0].BoundingSphere);
            //new BoundingBoxComponent(player, EntityFactory.CreateBoundingBox(modelComponent.Model)); 
            PlayerComponent playerComponent = new PlayerComponent(player);
            FrictionComponent frictionComponent = new FrictionComponent(player);
            TextureComponent textureComponent = new TextureComponent(player, texture);
            GravityComponent gravityComponent = new GravityComponent(player);

            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, collisionComponent, typeof(CollisionComponent));
            ComponentManager.Instance.AddComponentToEntity(player, playerComponent);
            ComponentManager.Instance.AddComponentToEntity(player, frictionComponent);
            ComponentManager.Instance.AddComponentToEntity(player, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gravityComponent);

            //TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            //TransformHelper.SetBoundingBoxPos(collisionComponent, transformComponent);

            TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            TransformHelper.SetInitialBoundingSpherePos(collisionComponent, transformComponent);

            return player;
        }
        //Create AI Player
        public static Entity NewAiPlayer(String model, int gamePadIndex, Vector3 transformPos, Texture2D texture)
        {
            Entity player = EntityFactory.NewEntity(GameEntityFactory.AI_PLAYER);
            TransformComponent transformComponent = new TransformComponent(player, transformPos);
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>(model));
            VelocityComponent velocityComponent = new VelocityComponent(player);
            CollisionComponent collisionComponent = new BoundingSphereComponent(player, modelComponent.Model.Meshes[0].BoundingSphere);
            //new BoundingBoxComponent(player, EntityFactory.CreateBoundingBox(modelComponent.Model));
            PlayerComponent playerComponent = new PlayerComponent(player);
            KeyboardComponent keyboardComponent = new KeyboardComponent(player);
            GamePadComponent gamePadComponent = new GamePadComponent(player, gamePadIndex);
            FrictionComponent frictionComponent = new FrictionComponent(player);
            TextureComponent textureComponent = new TextureComponent(player, texture);
            GravityComponent gravityComponent = new GravityComponent(player);
            AiComponent aiComponent = new AiComponent(player);
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
            ComponentManager.Instance.AddComponentToEntity(player, frictionComponent);
            ComponentManager.Instance.AddComponentToEntity(player, keyboardComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gamePadComponent);
            ComponentManager.Instance.AddComponentToEntity(player, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gravityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, aiComponent);

            //TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            //TransformHelper.SetBoundingBoxPos(collisionComponent, transformComponent);

            TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            TransformHelper.SetInitialBoundingSpherePos(collisionComponent, transformComponent);

            return player;
        }
        /// <summary>
        /// A Player without network component
        /// </summary>
        /// <param name="model"></param>
        /// <param name="gamePadIndex"></param>
        /// <param name="transformPos"></param>
        /// <param name="cameraPos"></param>
        /// <param name="cameraAspectRatio"></param>
        /// <param name="followPlayer"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Entity NewLocalPlayer(String model, int gamePadIndex, Vector3 transformPos, Vector3 cameraPos, float cameraAspectRatio, bool followPlayer, Texture2D texture)
        {
            Entity player = NewBasePlayer(model, gamePadIndex, transformPos, texture, LOCAL_PLAYER);
            CameraComponent cameraComponent = new CameraComponent(player, cameraPos, cameraAspectRatio, followPlayer);
            KeyboardComponent keyboardComponent = new KeyboardComponent(player);
            GamePadComponent gamePadComponent = new GamePadComponent(player, gamePadIndex);
            
            ComponentManager.Instance.AddComponentToEntity(player, cameraComponent);
            ComponentManager.Instance.AddComponentToEntity(player, keyboardComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gamePadComponent);
            

            return player;
        }

        public static Entity NewGoalBlock(Vector3 positionValues, Texture2D texture)
        {
            Entity player = NewBlock(positionValues, texture, GOAL);
            GoalComponent goalComponent = new GoalComponent(player);
            ComponentManager.Instance.AddComponentToEntity(player, goalComponent);
            return player;
        }

        public static Entity NewBlock(Vector3 positionValues, Texture2D texture, string typeName)
        {
            Entity block = EntityFactory.NewEntity(typeName);
            
            TransformComponent transformComponent = new TransformComponent(block, new Vector3(x: positionValues.X, y: positionValues.Y, z: positionValues.Z));
            ModelComponent modelComponent = new ModelComponent(block, AssetManager.Instance.GetContent<Model>("Models/block2"));
            modelComponent.World = Matrix.CreateWorld(transformComponent.Position, Vector3.Forward, Vector3.Up);
            TextureComponent textureComponent = new TextureComponent(block, texture);
            CollisionComponent collisionComponent = new BoundingBoxComponent(block, EntityFactory.CreateBoundingBox(modelComponent.Model));
            
            BlockComponent blockComponent = new BlockComponent(block);

            ComponentManager.Instance.AddComponentToEntity(block, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(block, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(block, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(block, collisionComponent, typeof(CollisionComponent));
            ComponentManager.Instance.AddComponentToEntity(block, blockComponent);

            TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            TransformHelper.SetBoundingBoxPos(collisionComponent, transformComponent);
            EntityFactory.AddBoundingBoxChildren((BoundingBoxComponent)collisionComponent);

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

            TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            TransformHelper.SetInitialBoundingSpherePos(boundingSphereComponent, transformComponent);
        }
    }
}
