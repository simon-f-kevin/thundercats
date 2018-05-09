using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Game_Engine.Systems;
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
        public static Entity NewPlayer(String model, int gamePadIndex, Vector3 transformPos, Texture2D texture)
        {
            Entity player = EntityFactory.NewEntity("local_player");
            TransformComponent transformComponent = new TransformComponent(player, transformPos);
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>(model));
            VelocityComponent velocityComponent = new VelocityComponent(player);
            BoundingSphereComponent boundingSphereComponent = new BoundingSphereComponent(player, modelComponent.Model.Meshes[0].BoundingSphere);
            PlayerComponent playerComponent = new PlayerComponent(player);
            KeyboardComponent keyboardComponent = new KeyboardComponent(player);
            GamePadComponent gamePadComponent = new GamePadComponent(player, gamePadIndex);
            FrictionComponent frictionComponent = new FrictionComponent(player);
            TextureComponent textureComponent = new TextureComponent(player, texture);

            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, boundingSphereComponent);
            ComponentManager.Instance.AddComponentToEntity(player, playerComponent);
            ComponentManager.Instance.AddComponentToEntity(player, keyboardComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gamePadComponent);
            ComponentManager.Instance.AddComponentToEntity(player, frictionComponent);
            ComponentManager.Instance.AddComponentToEntity(player, textureComponent);

            PhysicsSystem.SetInitialModelPos(modelComponent, transformComponent);
            PhysicsSystem.SetInitialBoundingSpherePos(boundingSphereComponent, transformComponent);

            return player;
        }
        public static Entity NewAiPlayer(String model, int gamePadIndex, Vector3 transformPos, Texture2D texture)
        {
            Entity player = EntityFactory.NewEntity("Ai_player");
            TransformComponent transformComponent = new TransformComponent(player, transformPos);
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>(model));
            VelocityComponent velocityComponent = new VelocityComponent(player);
            BoundingSphereComponent boundingSphereComponent = new BoundingSphereComponent(player, modelComponent.Model.Meshes[0].BoundingSphere);
            PlayerComponent playerComponent = new PlayerComponent(player);
            KeyboardComponent keyboardComponent = new KeyboardComponent(player);
            GamePadComponent gamePadComponent = new GamePadComponent(player, gamePadIndex);
            FrictionComponent frictionComponent = new FrictionComponent(player);
            TextureComponent textureComponent = new TextureComponent(player, texture);
            AiComponent aiComponent = new AiComponent(player);

            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, boundingSphereComponent);
            ComponentManager.Instance.AddComponentToEntity(player, playerComponent);
            ComponentManager.Instance.AddComponentToEntity(player, keyboardComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gamePadComponent);
            ComponentManager.Instance.AddComponentToEntity(player, frictionComponent);
            ComponentManager.Instance.AddComponentToEntity(player, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(player, aiComponent);

            PhysicsSystem.SetInitialModelPos(modelComponent, transformComponent);
            PhysicsSystem.SetInitialBoundingSpherePos(boundingSphereComponent, transformComponent);

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
            BoundingSphereComponent boundingSphereComponent = new BoundingSphereComponent(block, modelComponent.Model.Meshes[0].BoundingSphere);
            BlockComponent blockComponent = new BlockComponent(block);

            ComponentManager.Instance.AddComponentToEntity(block, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(block, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(block, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(block, boundingSphereComponent);
            ComponentManager.Instance.AddComponentToEntity(block, blockComponent);

            PhysicsSystem.SetInitialModelPos(modelComponent, transformComponent);
            PhysicsSystem.SetInitialBoundingSpherePos(boundingSphereComponent, transformComponent);

            return block;
        }

        public static void TestCollisionEntity(String model, Vector3 transformPos)
        {
            //Below is a temporary object you can use to test collision
            Entity blob = EntityFactory.NewEntity();
            ModelComponent modelComponent = new ModelComponent(blob, AssetManager.Instance.GetContent<Model>(model));
            TransformComponent transformComponent = new TransformComponent(blob, transformPos);
            BoundingSphereComponent boundingSphereComponent = new BoundingSphereComponent(blob, modelComponent.Model.Meshes[0].BoundingSphere);

            ComponentManager.Instance.AddComponentToEntity(blob, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, boundingSphereComponent);

            PhysicsSystem.SetInitialModelPos(modelComponent, transformComponent);
            PhysicsSystem.SetInitialBoundingSpherePos(boundingSphereComponent, transformComponent);
        }
    }
}
