using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Systems
{
    public class ModelRenderSystem : IDrawableSystem
    {
        public GraphicsDevice graphicsDevice { get; set; }
        public ModelRenderSystem()
        {

        }
        public void Draw(GameTime gameTime)
        {
            DrawModels(gameTime);
            //DrawGameWorld();
        }

        /*
         * Draws all Models in ModelComponents.
         * Requires a CameraComponent to exist on some Entity.
         */
        private void DrawModels(GameTime gameTime)
        {
            ConcurrentDictionary<Entity, Component> modelComponents = ComponentManager.Instance.GetConcurrentDictionary<ModelComponent>();
            ConcurrentDictionary<Entity, Component> cameraComponentPairs = ComponentManager.Instance.GetConcurrentDictionary<CameraComponent>();

            if (cameraComponentPairs.Count == 0)
            {
                return;
            }
            
            CameraComponent cameraComponent = (CameraComponent)cameraComponentPairs.First().Value;
            

            foreach (var modelComponentPair in modelComponents)
            {
                ModelComponent model = modelComponentPair.Value as ModelComponent;
                TextureComponent textureComponent = ComponentManager.Instance.ConcurrentGetComponentOfEntity<TextureComponent>(modelComponentPair.Key);
                model.BoneTransformations[0] = model.World;

                foreach(var modelMesh in model.Model.Meshes)
                {
                    foreach(BasicEffect effect in modelMesh.Effects)
                    {
                        effect.World = model.BoneTransformations[modelMesh.ParentBone.Index];
                        effect.View = cameraComponent.ViewMatrix;
                        effect.Projection = cameraComponent.ProjectionMatrix;
                        effect.EnableDefaultLighting();
                        effect.Texture = textureComponent.Texture;
                        effect.TextureEnabled = true;
                        //foreach(EffectPass ep in effect.CurrentTechnique.Passes)
                        //{
                        //    ep.Apply();
                        //    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
                        //}

                        modelMesh.Draw();
                    }
                }
            }
        }

        private void DrawGameWorld()
        {
            throw new NotImplementedException();
        }
    }
}
