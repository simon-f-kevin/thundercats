using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Entities;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Engine.Systems
{
    /*
    * System to handle all rendering of all 3D models.
    * ModelRenderSystem uses parallel foreach loops to improve performance, this does not require locks as the component manager is thread safe.
    */
    public class ModelRenderSystem : IDrawableSystem
    {
        
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
            ConcurrentDictionary<Entity, Component> modelComponentPairs = ComponentManager.Instance.GetComponentPairDictionary<ModelComponent>();
            ConcurrentDictionary<Entity, Component> cameraComponentPairs = ComponentManager.Instance.GetComponentPairDictionary<CameraComponent>();

            if(cameraComponentPairs.Count == 0)
            {
                return;
            }
            CameraComponent cameraComponent = (CameraComponent)cameraComponentPairs.First().Value;

            Parallel.ForEach(modelComponentPairs, modelComponentPair =>
            {
                ModelComponent model = modelComponentPair.Value as ModelComponent;
                model.BoneTransformations[0] = model.World;

                foreach(var modelMesh in model.Model.Meshes)
                {

                    foreach(BasicEffect effect in modelMesh.Effects)
                    {
                        effect.World = model.BoneTransformations[modelMesh.ParentBone.Index];
                        effect.View = cameraComponent.ViewMatrix;
                        effect.Projection = cameraComponent.ProjectionMatrix;
                        effect.EnableDefaultLighting();
                        modelMesh.Draw();
                    }
                }
            });
        }

        private void DrawGameWorld()
        {
            throw new NotImplementedException();
        }
    }
}
