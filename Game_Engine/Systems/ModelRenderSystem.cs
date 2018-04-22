using System;
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
    public class ModelRenderSystem : IDrawableSystem
    {
        
        public void Draw(GameTime gameTime)
        {
            DateTime oldTime = DateTime.Now;
            DrawModels(gameTime);
            DateTime newTime = DateTime.Now;
            Console.WriteLine((newTime - oldTime).Milliseconds);
            //DrawGameWorld();
        }

        /*
         * Draws all Models in ModelComponents.
         * Requires a CameraComponent to exist on some Entity.
         */
        private void DrawModels(GameTime gameTime)
        {
            Dictionary<Entity, Component> modelComponentPairs = ComponentManager.Instance.GetComponentPairDictionary<ModelComponent>();
            Dictionary<Entity, Component> cameraComponentPairs = ComponentManager.Instance.GetComponentPairDictionary<CameraComponent>();

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
