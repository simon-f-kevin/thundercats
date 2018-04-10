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
                var boneTransformations = new Matrix[model.Model.Bones.Count];
                model.Model.CopyAbsoluteBoneTransformsTo(boneTransformations);
                foreach(var modelMesh in model.Model.Meshes)
                {
                    foreach(BasicEffect effect in modelMesh.Effects)
                    {
                        effect.World = boneTransformations[modelMesh.ParentBone.Index];
                        effect.View = Matrix.CreateLookAt(Vector3.Backward, Vector3.Zero, Vector3.Up);
                        effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), (16 / 9), 1f, 1000f);
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
