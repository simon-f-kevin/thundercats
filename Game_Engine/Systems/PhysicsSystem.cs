using Game_Engine.Components;
using Game_Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine.Systems
{
    public class PhysicsSystem : IUpdateableSystem
    {
        public async void Update(GameTime gameTime)
        {
            RunGravity();
        }

        /// <summary>
        /// Applies gravity to all entities with a gravity-component and velocity-component. 
        /// If they have a velocity-compoent, but no gravity-component no gravity is applied. 
        /// </summary>
        private void RunGravity()
        {
            var gravityComponents = ComponentManager.Instance.GetComponentDictionary<GravityComponent>();
            foreach (var gravityComponentKeyValuePair in gravityComponents)
            {
                //if (!(gravityComponentKeyValuePair.Value is GravityComponent)) continue;
                var velocityComponent = ComponentManager.Instance.GetComponentOfEntity<VelocityComponent>(gravityComponentKeyValuePair.Key);
                 velocityComponent.Velocity.Y -= 0.5f;
            }
        }
    }
}
