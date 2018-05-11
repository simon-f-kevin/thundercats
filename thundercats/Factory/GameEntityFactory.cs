using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        public static Entity NewBasePlayer(String model, int gamePadIndex, Vector3 transformPos, Texture2D texture, string name = null)
        {
            Entity player;
            if (name == null)
            {
                player = EntityFactory.NewEntity(REMOTE_PLAYER);
            }
            else
            {
                player = EntityFactory.NewEntity(name);
            }
            TransformComponent transformComponent = new TransformComponent(player, transformPos);
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>(model));
            VelocityComponent velocityComponent = new VelocityComponent(player);
            CollisionComponent collisionComponent = new BoundingSphereComponent(player, modelComponent.Model.Meshes[0].BoundingSphere);
            PlayerComponent playerComponent = new PlayerComponent(player);
            FrictionComponent frictionComponent = new FrictionComponent(player);
            TextureComponent textureComponent = new TextureComponent(player, texture);

            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, collisionComponent, typeof(CollisionComponent));
            ComponentManager.Instance.AddComponentToEntity(player, playerComponent);
            ComponentManager.Instance.AddComponentToEntity(player, frictionComponent);
            ComponentManager.Instance.AddComponentToEntity(player, textureComponent);

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

        /// <summary>
        /// The host player in a multiplayer game vs human opponent
        /// </summary>
        /// <param name="model"></param>
        /// <param name="gamePadIndex"></param>
        /// <param name="transformPos"></param>
        /// <param name="cameraPos"></param>
        /// <param name="cameraAspectRatio"></param>
        /// <param name="followPlayer"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Entity NewLocalHostPlayer(String model, int gamePadIndex, Vector3 transformPos, Vector3 cameraPos, float cameraAspectRatio, bool followPlayer, Texture2D texture)
        {
            Entity player = NewLocalPlayer(model, gamePadIndex, transformPos, cameraPos, cameraAspectRatio, followPlayer, texture);
            
            return player;
        }

        /// <summary>
        /// The client player in a multiplayer game vs human opponent
        /// </summary>
        /// <param name="model"></param>
        /// <param name="gamePadIndex"></param>
        /// <param name="transformPos"></param>
        /// <param name="cameraPos"></param>
        /// <param name="cameraAspectRatio"></param>
        /// <param name="followPlayer"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Entity NewLocalClientPlayer(String model, int gamePadIndex, Vector3 transformPos, Vector3 cameraPos, float cameraAspectRatio, bool followPlayer, Texture2D texture)
        {
            Entity player = NewLocalPlayer(model, gamePadIndex, transformPos, cameraPos, cameraAspectRatio, followPlayer, texture);
            
            return player;
        }

        /// <summary>
        /// Human opponent in multiplayer game that is not the host
        /// </summary>
        /// <param name="model"></param>
        /// <param name="gamePadIndex"></param>
        /// <param name="texture"></param>
        /// <param name="transformPos"></param>
        /// <returns></returns>
        public static Entity NewRemotePlayer(String model, int gamePadIndex, Vector3 transformPos, Texture2D texture)
        {
            Entity player = NewBasePlayer(model, gamePadIndex, transformPos, texture, REMOTE_PLAYER);
            
            return player;
        }

        /// <summary>
        /// Human opponent in multiplayer game that is  the host
        /// </summary>
        /// <param name="model"></param>
        /// <param name="gamePadIndex"></param>
        /// <param name="texture"></param>
        /// <param name="transformPos"></param>
        /// <returns></returns>
        public static Entity NewRemoteHostPlayer(String model, int gamePadIndex, Vector3 transformPos, Texture2D texture)
        {
            Entity player = NewBasePlayer(model, gamePadIndex, transformPos, texture, REMOTE_PLAYER);
            
            return player;
        }

        /// <summary>
        /// Opponent in singleplayer game
        /// </summary>
        /// <param name="model"></param>
        /// <param name="gamePadIndex"></param>
        /// <param name="texture"></param>
        /// <param name="transformPos"></param>
        /// <returns></returns>
        public static Entity NewAiPlayer(String model, int gamePadIndex, Texture2D texture, Vector3 transformPos)
        {
            Entity player = NewBasePlayer(model, gamePadIndex, transformPos, texture, AI_PLAYER);
            //here be ai components in the future
            return player;
        }

        public static Entity NewGoalBlock(Vector3 positionValues, Texture2D texture)
        {
            Entity player = NewBlock(positionValues, texture, "Goal");
            GoalComponent goalComponent = new GoalComponent(player);
            ComponentManager.Instance.AddComponentToEntity(player, goalComponent);
            return player;
        }

        public static Entity NewBlock(Vector3 positionValues, Texture2D texture, string name = null)
        {
            Entity block;
            if (name == null)
            {
                block = EntityFactory.NewEntity();
            }
            else
            {
                block = EntityFactory.NewEntity(name);
            }
            TransformComponent transformComponent = new TransformComponent(block, new Vector3(x: positionValues.X, y: positionValues.Y, z: positionValues.Z));
            ModelComponent modelComponent = new ModelComponent(block, AssetManager.Instance.GetContent<Model>("Models/Block"));
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
            TransformHelper.SetInitialBoundingBoxPos(collisionComponent, transformComponent);
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
