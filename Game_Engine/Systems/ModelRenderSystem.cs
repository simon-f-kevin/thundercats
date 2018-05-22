﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Helpers;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Systems
{
    /*
    * System to handle all rendering of all 3D models.
    * ModelRenderSystem uses parallel foreach loops to improve performance, this does not require locks as the component manager is thread safe.
    */
    public class ModelRenderSystem : IDrawableSystem, IUpdateableSystem
    {
        public GraphicsDevice graphicsDevice { get; set; }
        public ModelRenderSystem()
        {

        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(GameTime gameTime)
        {
            DrawModelsWithEffect(gameTime);
        }

        /*
         * Draws all Models in ModelComponents.
         * Requires a CameraComponent to exist on some Entity.
         */
        private void DrawModels(GameTime gameTime)
        {
            //Undoes any changes from using the spritebatch
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;


            ConcurrentDictionary<Entity, Component> modelComponents = ComponentManager.Instance.GetConcurrentDictionary<ModelComponent>();
            ConcurrentDictionary<Entity, Component> cameraComponentPairs = ComponentManager.Instance.GetConcurrentDictionary<CameraComponent>();

            if (cameraComponentPairs.Count == 0)
            {
                return;
            }

            CameraComponent cameraComponent = (CameraComponent)cameraComponentPairs.First().Value; //get the cameracomponent for the local player

            foreach (var modelComponentPair in modelComponents)
            {
                ModelComponent model = modelComponentPair.Value as ModelComponent;
                var collisionComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CollisionComponent>(modelComponentPair.Key);
                if (cameraComponent.BoundingFrustum.Intersects(collisionComponent.BoundingShape))
                {
                    TextureComponent textureComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TextureComponent>(modelComponentPair.Key);
                    model.BoneTransformations[0] = model.World;

                    foreach (var modelMesh in model.Model.Meshes)
                    {
                        foreach (BasicEffect effect in modelMesh.Effects)
                        {
                            effect.World = model.BoneTransformations[modelMesh.ParentBone.Index] * EngineHelper.Instance().WorldMatrix;
                            effect.View = cameraComponent.ViewMatrix;
                            effect.Projection = cameraComponent.ProjectionMatrix;
                            effect.EnableDefaultLighting();
                            effect.LightingEnabled = true;
                            if (textureComponent != null)
                            {
                                effect.Texture = textureComponent.Texture;
                                effect.TextureEnabled = true;
                            }
                            modelMesh.Draw();
                        }
                    }
                }
            }

            //Parallel.ForEach(modelComponents, modelComponentPair =>
            //{
            //    ModelComponent model = modelComponentPair.Value as ModelComponent;
            //    TextureComponent textureComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TextureComponent>(modelComponentPair.Key);
            //    model.BoneTransformations[0] = model.World;

            //    foreach(var modelMesh in model.Model.Meshes)
            //    {
            //        foreach(BasicEffect effect in modelMesh.Effects)
            //        {
            //            effect.World = model.BoneTransformations[modelMesh.ParentBone.Index];
            //            effect.View = cameraComponent.ViewMatrix;
            //            effect.Projection = cameraComponent.ProjectionMatrix;
            //            effect.EnableDefaultLighting();
            //            effect.LightingEnabled = true;
            //            effect.Texture = textureComponent.Texture;
            //            effect.TextureEnabled = true
            //            modelMesh.Draw();
            //        }
            //    }
            //});
        }

        private void DrawModelsWithEffect(GameTime gameTime)
        {
            //Undoes any changes from using the spritebatch
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            ConcurrentDictionary<Entity, Component> modelComponents = ComponentManager.Instance.GetConcurrentDictionary<ModelComponent>();
            ConcurrentDictionary<Entity, Component> cameraComponentPairs = ComponentManager.Instance.GetConcurrentDictionary<CameraComponent>();

            if (cameraComponentPairs.Count == 0)
            {
                return;
            }

            CameraComponent cameraComponent = (CameraComponent)cameraComponentPairs.First().Value; //get the cameracomponent for the local player

            foreach (var modelComponentPair in modelComponents)
            {
                ModelComponent model = modelComponentPair.Value as ModelComponent;
                var collisionComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<CollisionComponent>(modelComponentPair.Key);
                if (cameraComponent.BoundingFrustum.Intersects(collisionComponent.BoundingShape))
                {
                    TextureComponent textureComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TextureComponent>(modelComponentPair.Key);
                    EffectComponent effectComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<EffectComponent>(modelComponentPair.Key);
                    LightComponent lightComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<LightComponent>(modelComponentPair.Key);
                    TransformComponent transformcComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TransformComponent>(modelComponentPair.Key);


                    model.BoneTransformations[0] = model.World;

                    foreach (var modelMesh in model.Model.Meshes)
                    {
                        foreach (ModelMeshPart part in modelMesh.MeshParts)
                        {
                            part.Effect = effectComponent.Effect;
                            part.Effect.Parameters["World"].SetValue(model.BoneTransformations[modelMesh.ParentBone.Index] * EngineHelper.Instance().WorldMatrix);
                            part.Effect.Parameters["View"].SetValue(cameraComponent.ViewMatrix);
                            part.Effect.Parameters["Projection"].SetValue(cameraComponent.ProjectionMatrix);
                            part.Effect.Parameters["SpecularColor"].SetValue(Color.White.ToVector4());
                            part.Effect.Parameters["AmbientColor"].SetValue(Color.Black.ToVector4());
                            part.Effect.Parameters["AmbientIntensity"].SetValue(1f);
                            part.Effect.Parameters["DiffuseColor"].SetValue(Color.DarkGray.ToVector4());
                            part.Effect.Parameters["DiffuseIntensity"].SetValue(5f);
                            part.Effect.Parameters["LightDirection"].SetValue(transformcComponent.Position);
                            part.Effect.Parameters["EyePosition"].SetValue(transformcComponent.Position);
                        }
                        modelMesh.Draw();
                    }
                }
            }
        }

        //Parallel.ForEach(modelComponents, modelComponentPair =>
        //{
        //    ModelComponent model = modelComponentPair.Value as ModelComponent;
        //    TextureComponent textureComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TextureComponent>(modelComponentPair.Key);
        //    model.BoneTransformations[0] = model.World;

        //    foreach(var modelMesh in model.Model.Meshes)
        //    {
        //        foreach(BasicEffect effect in modelMesh.Effects)
        //        {
        //            effect.World = model.BoneTransformations[modelMesh.ParentBone.Index];
        //            effect.View = cameraComponent.ViewMatrix;
        //            effect.Projection = cameraComponent.ProjectionMatrix;
        //            effect.EnableDefaultLighting();
        //            effect.LightingEnabled = true;
        //            effect.Texture = textureComponent.Texture;
        //            effect.TextureEnabled = true
        //            modelMesh.Draw();
        //        }
        //    }
        //});
    }
}


