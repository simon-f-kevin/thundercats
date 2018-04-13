using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;

namespace Game_Engine.Systems
{
    public class MovementSystem : IUpdateableSystem
    {
        public void Update(GameTime gameTime)
        {
            var velocityComponents = ComponentManager.Instance.GetComponentDictionary<VelocityComponent>();
            foreach(var velocityComponentPair in velocityComponents)
            {
                var velocityComponent = velocityComponentPair.Value as VelocityComponent;
                var transformationComponent = ComponentManager.Instance.GetComponentOfEntity<TransformComponent>(velocityComponentPair.Key);
                var modelComponent = ComponentManager.Instance.GetComponentOfEntity<ModelComponent>(velocityComponentPair.Key);

                transformationComponent.position += velocityComponent.Velocity;
                if (modelComponent != null)
                {
                    //UpdateModel(modelComponent, transformation, velocity);
                    modelComponent.Model.Bones[0].Transform *= Matrix.CreateTranslation(velocityComponent.Velocity.X, velocityComponent.Velocity.Y, velocityComponent.Velocity.Z) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(velocityComponent.Velocity.X, velocityComponent.Velocity.Y, velocityComponent.Velocity.Z);

                }
            }
        }
        public void UpdateModel(ModelComponent modelComponent, TransformComponent transformComponent, VelocityComponent velocityComponent)
        {
            modelComponent.Model.Bones[0].Transform *= Matrix.CreateTranslation(velocityComponent.Velocity.X, 0, 0) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(velocityComponent.Velocity.X, 0, 0);
            modelComponent.Model.Bones[0].Transform *= Matrix.CreateTranslation(0, velocityComponent.Velocity.Y, 0) * Matrix.CreateRotationY(0) * Matrix.CreateTranslation(0, velocityComponent.Velocity.Y, 0);
            modelComponent.Model.Bones[0].Transform *= Matrix.CreateTranslation(0, 0, velocityComponent.Velocity.Z) * Matrix.CreateRotationZ(0) * Matrix.CreateTranslation(0, 0, velocityComponent.Velocity.Z);
        }
    }
}
