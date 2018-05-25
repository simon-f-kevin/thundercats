using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;

namespace Game_Engine.Systems.Physics
{
    public class GravitySystem : IUpdateableSystem
    {
        /// <summary>
        /// Applies gravity to all entities with a gravity-component and velocity-component. 
        /// If they have a velocity-component, but no gravity-component no gravity is applied. 
        /// </summary>
        public void Update(GameTime gameTime)
        {
            var gravityComponents = ComponentManager.Instance.GetConcurrentDictionary<GravityComponent>();

            Parallel.ForEach(gravityComponents, gravityComponentKeyValuePair =>
                {
                    var gravityComponent = gravityComponentKeyValuePair.Value as GravityComponent;
                    var velocityComponent =
                        ComponentManager.Instance.ConcurrentGetComponentOfEntity<VelocityComponent>(
                            gravityComponentKeyValuePair.Key);

                    if (gravityComponent != null)
                    {
                        var acceleration = (gravityComponent.GravityCoefficient / gravityComponent.Mass) * 0.05f;
                        velocityComponent.Velocity.Y -= acceleration;
                    }
                }
            );
        }
    }
}
