﻿using Game_Engine.Components;
using Game_Engine.Components.Preformance;
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
        public const string OUTOFBOUNDS = "Out of bounds";

        public static GraphicsDevice GraphicsDevice;

        public static Entity NewBasePlayer(String model, int gamePadIndex, Vector3 transformPos, Texture2D texture, String typeName)
        {
            Entity player = EntityFactory.NewEntity(typeName);
            TransformComponent transformComponent = new TransformComponent(player, transformPos);
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>(model));
            VelocityComponent velocityComponent = new VelocityComponent(player);
            CollisionComponent collisionComponent = new CollisionComponent(player, new BoxVolume(EntityFactory.CreateBoundingBox(modelComponent.Model)));
            PlayerComponent playerComponent = new PlayerComponent(player);
            FrictionComponent frictionComponent = new FrictionComponent(player);
            TextureComponent textureComponent = new TextureComponent(player, texture);
            GravityComponent gravityComponent = new GravityComponent(player);
            EffectComponent effectComponent = new EffectComponent(player, AssetManager.Instance.GetContent<Effect>("Shading"));

            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, collisionComponent, typeof(CollisionComponent));
            ComponentManager.Instance.AddComponentToEntity(player, playerComponent);
            ComponentManager.Instance.AddComponentToEntity(player, frictionComponent);
            ComponentManager.Instance.AddComponentToEntity(player, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gravityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, effectComponent);

            TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            TransformHelper.SetBoundingBoxPos(collisionComponent, transformComponent);

            //TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            //TransformHelper.SetInitialBoundingSpherePos(collisionComponent, transformComponent);

            return player;
        }
        //Create AI Player
        public static Entity NewAiPlayer(String model, Vector3 transformPos, Texture2D texture)
        {
            Entity player = EntityFactory.NewEntity(GameEntityFactory.AI_PLAYER);
            TransformComponent transformComponent = new TransformComponent(player, transformPos);
            ModelComponent modelComponent = new ModelComponent(player, AssetManager.Instance.GetContent<Model>(model));
            VelocityComponent velocityComponent = new VelocityComponent(player);
            CollisionComponent collisionComponent = new CollisionComponent(player, new BoxVolume(EntityFactory.CreateBoundingBox(modelComponent.Model)));
            PlayerComponent playerComponent = new PlayerComponent(player);
            FrictionComponent frictionComponent = new FrictionComponent(player);
            TextureComponent textureComponent = new TextureComponent(player, texture);
            GravityComponent gravityComponent = new GravityComponent(player);
            AiComponent aiComponent = new AiComponent(player);
            LightComponent lightComponent = new LightComponent(player, new Vector3(0, 7, -5), Color.White.ToVector4(), 10f, Color.Blue.ToVector4(), 0.2f, Color.White.ToVector4(), 1000f);
            EffectComponent effectComponent = new EffectComponent(player, AssetManager.Instance.GetContent<Effect>("Shading"));




            ComponentManager.Instance.AddComponentToEntity(player, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(player, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(player, velocityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, collisionComponent, typeof(CollisionComponent));
            ComponentManager.Instance.AddComponentToEntity(player, playerComponent);
            ComponentManager.Instance.AddComponentToEntity(player, frictionComponent);
            ComponentManager.Instance.AddComponentToEntity(player, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gravityComponent);
            ComponentManager.Instance.AddComponentToEntity(player, aiComponent);
            ComponentManager.Instance.AddComponentToEntity(player, effectComponent);
            ComponentManager.Instance.AddComponentToEntity(player, lightComponent);

            //TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            //TransformHelper.SetBoundingBoxPos(collisionComponent, transformComponent);

            TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            TransformHelper.SetBoundingBoxPos(collisionComponent, transformComponent);

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
            FPSComponent fpsComponent = new FPSComponent(player);
            UIComponent spriteFPSCounterComponent = new UIComponent(player) { Position = new Vector2(10, 10),
                Text = fpsComponent.CurrentFramesPerSecond.ToString(),
                Color = Color.White,
                SpriteFont = AssetManager.Instance.GetContent<SpriteFont>("menu") };
            NetworkDiagnosticComponent networkDiagnosticComponent = new NetworkDiagnosticComponent(player);

            ComponentManager.Instance.AddComponentToEntity(player, cameraComponent);
            ComponentManager.Instance.AddComponentToEntity(player, keyboardComponent);
            ComponentManager.Instance.AddComponentToEntity(player, gamePadComponent);
            ComponentManager.Instance.AddComponentToEntity(player, fpsComponent);
            ComponentManager.Instance.AddComponentToEntity(player, networkDiagnosticComponent);
            ComponentManager.Instance.AddComponentToEntity(player, spriteFPSCounterComponent);
            

            return player;
        }

        public static Entity NewGoalBlock(Vector3 positionValues, Texture2D texture)
        {
            Entity player = NewBlock(positionValues, texture, GOAL);
            GoalComponent goalComponent = new GoalComponent(player);
            ComponentManager.Instance.AddComponentToEntity(player, goalComponent);
            return player;
        }
        public static Entity NewOutOfBounds(Vector3 positionStart, Vector3 positionEnd) {
            Entity outOfBounds = EntityFactory.NewEntity(GameEntityFactory.OUTOFBOUNDS);
            CollisionComponent box = new CollisionComponent(outOfBounds, new BoxVolume(new BoundingBox(positionStart, positionEnd)));

            ComponentManager.Instance.AddComponentToEntity(outOfBounds, box, typeof(CollisionComponent));
            return outOfBounds;
        }

        public static Entity NewBlock(Vector3 positionValues, Texture2D texture, string typeName)
        {
            Entity block = EntityFactory.NewEntity(typeName);
            
            TransformComponent transformComponent = new TransformComponent(block, new Vector3(x: positionValues.X, y: positionValues.Y, z: positionValues.Z));
            ModelComponent modelComponent = new ModelComponent(block, AssetManager.Instance.GetContent<Model>("Models/block2"));
            modelComponent.World = Matrix.CreateWorld(transformComponent.Position, Vector3.Forward, Vector3.Up);
            TextureComponent textureComponent = new TextureComponent(block, texture);
            CollisionComponent collisionComponent = new CollisionComponent(block, new BoxVolume(EntityFactory.CreateBoundingBox(modelComponent.Model)));

            LightComponent lightComponent = new LightComponent(block, new Vector3(0, 7, -5), Color.White.ToVector4(), 10f, Color.Blue.ToVector4(), 0.2f, Color.White.ToVector4(), 1000f);
            EffectComponent effectComponent = new EffectComponent(block, AssetManager.Instance.GetContent<Effect>("Shading"));

            BlockComponent blockComponent = new BlockComponent(block);

            ComponentManager.Instance.AddComponentToEntity(block, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(block, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(block, textureComponent);
            ComponentManager.Instance.AddComponentToEntity(block, collisionComponent, typeof(CollisionComponent));
            ComponentManager.Instance.AddComponentToEntity(block, blockComponent);
            ComponentManager.Instance.AddComponentToEntity(block, effectComponent);
            ComponentManager.Instance.AddComponentToEntity(block, lightComponent);

            TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            TransformHelper.SetBoundingBoxPos(collisionComponent, transformComponent);
            //EntityFactory.AddBoundingBoxChildren((BoxVolume)collisionComponent);

            return block;
        }

        public static void TestCollisionEntity(String model, Vector3 transformPos)
        {
            //Below is a temporary object you can use to test collision
            Entity blob = EntityFactory.NewEntity();
            ModelComponent modelComponent = new ModelComponent(blob, AssetManager.Instance.GetContent<Model>(model));
            TransformComponent transformComponent = new TransformComponent(blob, transformPos);
            //CollisionComponent boundingSphereComponent = new SphereVolume(blob, modelComponent.Model.Meshes[0].BoundingSphere);
            CollisionComponent boundingSphereComponent = new CollisionComponent(blob, new SphereVolume(modelComponent.Model.Meshes[0].BoundingSphere));

            ComponentManager.Instance.AddComponentToEntity(blob, modelComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, transformComponent);
            ComponentManager.Instance.AddComponentToEntity(blob, boundingSphereComponent, typeof(CollisionComponent));

            TransformHelper.SetInitialModelPos(modelComponent, transformComponent);
            TransformHelper.SetInitialBoundingSpherePos(boundingSphereComponent, transformComponent);
        }

        public static Entity NewParticleSettingsEntity(Entity player, int maxParticles, float lifeSpan, string textureName)
        {
            Entity particles = player;
            ParticleSettingsComponent particleSettingsComponent = new ParticleSettingsComponent(particles)
            {
                AlphaBlendState = BlendState.Additive,
                EmitterVelocitySensitivity = 1,
                GravityDirection = Vector3.Up,
                MaximumParticles = maxParticles,
                MaxVelocity = 10,
                MinVelocity = 0,
                MinColor = new Color(255, 255, 255, 100),
                MaxColor = new Color(255, 255, 255, 100),
                ParticleLifeSpan = TimeSpan.FromSeconds(lifeSpan),
                MaxSize = 100,
                MinSize = 100,
                TextureName = textureName
            };

            ComponentManager.Instance.AddComponentToEntity(particles, particleSettingsComponent);
            return particles;
        }
    }
}
