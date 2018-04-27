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

                transformationComponent.Position.New = velocityComponent.Velocity.Old;
                if (modelComponent != null)
                {
                    //UpdateModel(modelComponent, transformation, velocity);
                    modelComponent.World *= Matrix.CreateTranslation(velocityComponent.Velocity.Old.X, velocityComponent.Velocity.Old.Y, velocityComponent.Velocity.Old.Z) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(velocityComponent.Velocity.Old.X, velocityComponent.Velocity.Old.Y, velocityComponent.Velocity.Old.Z);
                }
                Console.WriteLine(transformationComponent.Position.ToString());
            }
        }
    }
}
