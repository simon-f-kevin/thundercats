using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
            TextureComponent textureComponent = new TextureComponent(player)
            {
                Texture = texture
            };

            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, boundingSphereComponent);
            ComponentManager.Instance.AddComponentToEntity(player, playerComponent);
            //ComponentManager.Instance.AddComponentToEntity(player, keyboardComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gamePadComponent);
            ComponentManager.Instance.AddComponentToEntity(player, textureComponent);

            return player;
        }

        public static Entity NewPlayerWithCamera(String model, int gamePadIndex, Vector3 transformPos, Vector3 cameraPos, float cameraAspectRatio, bool followPlayer, Texture2D texture)
        {
            Entity player = EntityFactory.NewEntity("local_player");
            TransformComponent transformComponent = new TransformComponent(player, transformPos);
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>(model));
            VelocityComponent velocityComponent = new VelocityComponent(player);
            BoundingSphereComponent boundingSphereComponent = new BoundingSphereComponent(player, modelComponent.Model.Meshes[0].BoundingSphere);
            PlayerComponent playerComponent = new PlayerComponent(player);
            KeyboardComponent keyboardComponent = new KeyboardComponent(player);
            GamePadComponent gamePadComponent = new GamePadComponent(player, gamePadIndex);
            CameraComponent cameraComponent = new CameraComponent(player, cameraPos, cameraAspectRatio, followPlayer);
            FrictionComponent frictionComponent = new FrictionComponent(player);
            TextureComponent textureComponent = new TextureComponent(player)
            {
                Texture = texture
            };

            ComponentManager.Instance.AddComponentToEntity(player, cameraComponent);
            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, boundingSphereComponent);
            ComponentManager.Instance.AddComponentToEntity(player, playerComponent);
            ComponentManager.Instance.AddComponentToEntity(player, keyboardComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gamePadComponent);
            ComponentManager.Instance.AddComponentToEntity(player, frictionComponent);
            ComponentManager.Instance.AddComponentToEntity(player, textureComponent);

            return player;
        }

        public static Entity NewBlock(Vector2 positionValues, Texture2D Texture)
        {
            Entity block = EntityFactory.NewEntity();
            TransformComponent transformComponent = new TransformComponent(block, new Vector3(x: positionValues.X, y: 0, z: positionValues.Y));
            ModelComponent modelComponent = new ModelComponent(block, AssetManager.Instance.GetContent<Model>("Models/Block"));
            modelComponent.World = Matrix.CreateWorld(transformComponent.Position, Vector3.Forward, Vector3.Up);
            TextureComponent textureComponent = new TextureComponent(block)
            {
                Texture = Texture
            };
            BlockComponent blockComponent = new BlockComponent(block);

            ComponentManager.Instance.AddComponentToEntity(block, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(block, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(block, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(block, blockComponent);

            return block;
        }
    }
}
