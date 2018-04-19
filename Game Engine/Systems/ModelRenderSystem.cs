using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
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

        private void DrawModels(GameTime gameTime)
        {
            var models = ComponentManager.Instance.GetComponentDictionary<ModelComponent>();

            foreach(var modelKeyValuePair in models)
            {
                ModelComponent model = modelKeyValuePair.Value as ModelComponent;
                var transformComponent = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(modelKeyValuePair.Key);
                var textureComponent = ComponentManager.Instance.GetComponentOfEntity<TextureComponent>(modelKeyValuePair.Key);
                var cameraComponent = ComponentManager.Instance.GetComponentOfEntity<CameraComponent>(modelKeyValuePair.Key);
                model.BoneTransformations[0] = model.World;
                foreach (var modelMesh in model.Model.Meshes)
                {
                    var hej = modelMesh.MeshParts;

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
